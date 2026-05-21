#region Namespaces

#endregion

using System.ComponentModel;
namespace CABCommunication.Common
{
    /// <summary>
    /// Specifies the result status of communication.
    /// </summary>
    public enum CommunicationErrorType
    {
        /// <summary>
        /// When Data is received properly without any timeout.
        /// </summary>
        [Description("Success.")]
        Success = 0,

        /// <summary>
        /// When port specified is not available.
        /// </summary>
        [Description("Port Not Available.")]
        PortInvalid,

        /// <summary>
        /// When data received is more than the capacity of receive Buffer.
        /// </summary>
        [Description("")]
        BufferOverFlow,

        /// <summary>
        /// When Response time out happened.
        /// </summary>
        [Description("Sign On Failure/Timeout.")]
        ResponseTimeout,

        /// <summary>
        /// When Inter frame timeout time out happened.
        /// </summary>
        [Description("Sign On Failure.")]
        InterFrameTimeout,

        /// <summary>
        /// When start or end tag is invalid
        /// </summary>
        [Description("Sign On Failure.")]
        InvalidTag = 10,

        /// <summary>
        /// When FCS is invalid
        /// </summary>
        [Description("Sign On Failure.")]
        InvalidFCS,

        /// <summary>
        /// When Serevr SAP is invalid
        /// </summary>
        [Description("Sign On Failure.")]
        InvalidServerAddress,

        /// <summary>
        /// When command is invalid
        /// </summary>
        [Description("Sign On Failure.")]
        InvalidCommand,

        /// <summary>
        /// When get response tag is invalid
        /// </summary>
        [Description("Sign On Failure.")]
        InvalidGetResponseTag = 20,


        /// <summary>
        /// When AARQ tag is invalid
        /// </summary>
        [Description("Sign On Failure.")]
        InvalidAAREtag,

        /// <summary>
        /// When password is invalid
        /// </summary>
        [Description("Wrong Password.")]
        PasswordInavalid,

        /// <summary>
        /// When next block is transferring
        /// </summary>
        [Description("")]
        BlockTransferNext,

        /// <summary>
        /// When last block is transferring
        /// </summary>
        [Description("")]
        BlockTransferLast,

        /// <summary>
        /// When meter sends access denied
        /// </summary>
        [Description("Access Denied.")]
        AccessDenied,

        /// <summary>
        /// When meter sends Success for IEC
        /// </summary>
        [Description("")]
        SuccessForIEC,
        /// <summary>
        /// When meter sends Success for IEC
        /// </summary>
        [Description("")]
        SuccessForDLMS,
        /// <summary>
        /// When local modem is not connected
        /// </summary>
        [Description("Local modem connected.")]
        LocalModemConnected,
        /// <summary>
        /// Local modem is not connected
        /// </summary>
        [Description("Local modem not connected.")]
        LocalModemNotConnected,

        /// <summary>
        /// Remote modem not connected
        /// </summary>
        [Description("Remote modem not connected.")]
        RemoteModemNotConnected,

        /// <summary>
        /// While doing remote connection, if connected to a DLMS modem
        /// </summary>
        [Description("Connected to DLMS meter.")]
        ConnectedDLMS,

        /// <summary>
        /// While doing remote communication, if connected on Non-DLMS modem
        /// </summary>
        [Description("Connected to Non-DLMS meter.")]
        ConnectedNonDLMS,


        /// <summary>
        /// While doing communication, if fast download mode is not suuported in the device
        /// </summary>
        [Description("Fast download mode not supported.")]
        FastDownloadNotSupported,


        /// <summary>
        /// If Meter ID value is corrupt, then so this message.
        /// </summary>
        [Description("Invalid Meter ID.")]
        InvalidMeterIDName,

        /// <summary>
        /// For GPRS Communication when no reponse received from modem/meter
        /// </summary>
        [Description("Connection with modem/meter is not available")]
        ModemORMeterNotConnected,

        /// <summary>
        /// When meter sends Success for IEC Single Phase 
        /// </summary>
        [Description("")]
        SuccessForIECSP,

        /// <summary>
        /// No specific messsage
        /// </summary>
        [Description("")]
        Nothing = 5,
        /// <summary>
        /// No specific messsage
        /// </summary>
        [Description("Cosem Connection Failed")]
        CosemConnectionFailed

    }

}
