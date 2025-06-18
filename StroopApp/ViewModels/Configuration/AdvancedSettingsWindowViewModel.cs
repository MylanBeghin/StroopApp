using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows;

using CommunityToolkit.Mvvm.Input;

public class AdvancedSettingsWindowViewModel : INotifyPropertyChanged
{
	public ObservableCollection<string> SerialPorts { get; } = new();
	public ObservableCollection<int> BaudRates { get; } = new() { 9600, 19200, 38400, 57600, 115200 };
	public ObservableCollection<Parity> Parities { get; } = new() { Parity.None, Parity.Even, Parity.Odd, Parity.Mark, Parity.Space };
	public ObservableCollection<StopBits> StopBitsOptions { get; } = new() { StopBits.One, StopBits.OnePointFive, StopBits.Two };

	private string _selectedPort;
	public string SelectedPort
	{
		get => _selectedPort;
		set
		{
			if (_selectedPort != value)
			{
				_selectedPort = value;
				OnPropertyChanged(nameof(SelectedPort));
				OnPropertyChanged(nameof(IsPortSelected));
			}
		}
	}

	private int _selectedBaudRate = 9600;
	public int SelectedBaudRate
	{
		get => _selectedBaudRate;
		set
		{
			if (_selectedBaudRate != value)
			{
				_selectedBaudRate = value;
				OnPropertyChanged(nameof(SelectedBaudRate));
			}
		}
	}

	private Parity _selectedParity = Parity.None;
	public Parity SelectedParity
	{
		get => _selectedParity;
		set
		{
			if (_selectedParity != value)
			{
				_selectedParity = value;
				OnPropertyChanged(nameof(SelectedParity));
			}
		}
	}

	private StopBits _selectedStopBits = StopBits.One;
	public StopBits SelectedStopBits
	{
		get => _selectedStopBits;
		set
		{
			if (_selectedStopBits != value)
			{
				_selectedStopBits = value;
				OnPropertyChanged(nameof(SelectedStopBits));
			}
		}
	}

	public bool IsPortSelected => !string.IsNullOrWhiteSpace(SelectedPort);

	private string _connectionStatus = "Déconnecté";
	public string ConnectionStatus
	{
		get => _connectionStatus;
		set
		{
			if (_connectionStatus != value)
			{
				_connectionStatus = value;
				OnPropertyChanged(nameof(ConnectionStatus));
			}
		}
	}

	private string _connectButtonText = "Se connecter";
	public string ConnectButtonText
	{
		get => _connectButtonText;
		set
		{
			if (_connectButtonText != value)
			{
				_connectButtonText = value;
				OnPropertyChanged(nameof(ConnectButtonText));
			}
		}
	}

	private string _messageLog = string.Empty;
	public string MessageLog
	{
		get => _messageLog;
		set
		{
			if (_messageLog != value)
			{
				_messageLog = value;
				OnPropertyChanged(nameof(MessageLog));
			}
		}
	}

	private string _messageToSend = string.Empty;
	public string MessageToSend
	{
		get => _messageToSend;
		set
		{
			if (_messageToSend != value)
			{
				_messageToSend = value;
				OnPropertyChanged(nameof(MessageToSend));
				((CommunityToolkit.Mvvm.Input.RelayCommand)SendCommand).NotifyCanExecuteChanged();
			}
		}


	}

	public IRelayCommand RefreshCommand
	{
		get;
	}
	public IRelayCommand ConnectCommand
	{
		get;
	}
	public IRelayCommand SendCommand
	{
		get;
	}
	public IRelayCommand CopyLogCommand
	{
		get;
	}
	public IRelayCommand ClearLogCommand
	{
		get;
	}


	private SerialPort _serialPort;

	public AdvancedSettingsWindowViewModel()
	{
		RefreshCommand = new RelayCommand(RefreshSerialPorts);
		ConnectCommand = new RelayCommand(ConnectOrDisconnect);
		SendCommand = new RelayCommand(SendMessage, CanSendMessage);
		CopyLogCommand = new RelayCommand(CopyLogToClipboard);
		ClearLogCommand = new RelayCommand(ClearLog);


		RefreshSerialPorts();
	}

	private void RefreshSerialPorts()
	{
		SerialPorts.Clear();
		foreach (var port in SerialPort.GetPortNames())
			SerialPorts.Add(port);

		if (SerialPorts.Count > 0 && SelectedPort == null)
			SelectedPort = SerialPorts[0];
#if DEBUG
		// Simulation si aucun port trouvé
		if (SerialPorts.Count == 0)
		{
			SerialPorts.Add("COM1");
			SerialPorts.Add("COM2");
			SerialPorts.Add("COM3");
			SerialPorts.Add("SIM-USB0");
		}
#endif
	}

	private void ConnectOrDisconnect()
	{
		if (_serialPort == null || !_serialPort.IsOpen)
		{
			if (string.IsNullOrEmpty(SelectedPort))
			{
				MessageBox.Show("Aucun port sélectionné.");
				return;
			}

			try
			{
				_serialPort = new SerialPort(SelectedPort, SelectedBaudRate, SelectedParity, 8, SelectedStopBits);
				_serialPort.DataReceived += SerialPort_DataReceived;
				_serialPort.Open();
				ConnectionStatus = $"Connecté à {SelectedPort}";
				ConnectButtonText = "Se déconnecter";
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Erreur lors de la connexion : {ex.Message}");
			}
		}
		else
		{
			try
			{
				_serialPort.DataReceived -= SerialPort_DataReceived;
				_serialPort.Close();
				_serialPort = null;
				ConnectionStatus = "Déconnecté";
				ConnectButtonText = "Se connecter";
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Erreur lors de la déconnexion : {ex.Message}");
			}
		}

		// Toujours notifier le changement de possibilité d’envoi !
		SendCommand.NotifyCanExecuteChanged();
	}


	private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		try
		{
			var sp = (SerialPort)sender;
			var data = sp.ReadExisting();
			Application.Current.Dispatcher.Invoke(() =>
			{
				MessageLog += $"[IN] {data}{Environment.NewLine}";
			});
		}
		catch (Exception ex)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				MessageLog += $"[ERREUR] {ex.Message}{Environment.NewLine}";
			});
		}
	}

	private void SendMessage()
	{
		if (_serialPort != null && _serialPort.IsOpen)
		{
			try
			{
				_serialPort.WriteLine(MessageToSend);
				MessageLog += $"[OUT] {MessageToSend}{Environment.NewLine}";
				MessageToSend = string.Empty;
			}
			catch (Exception ex)
			{
				MessageLog += $"[ERREUR] {ex.Message}{Environment.NewLine}";
			}
		}
	}

	private bool CanSendMessage()
	{
		return _serialPort != null && _serialPort.IsOpen && !string.IsNullOrWhiteSpace(MessageToSend);
	}

	private void CopyLogToClipboard()
	{
		Clipboard.SetText(MessageLog);
	}

	private void ClearLog()
	{
		MessageLog = string.Empty;
	}
	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
