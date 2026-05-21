# Cabcon Edge Solution (3-Phase BCS)

![Cabcon Edge Solution](https://img.shields.io/badge/Platform-.NET_WinForms-blue.svg)
![Status](https://img.shields.io/badge/Status-Active_Development-brightgreen.svg)


**Cabcon Edge Solution** is an enterprise-grade Base Computer Software (BCS) designed for advanced electrical meter data acquisition, configuration, and comprehensive reporting. Originally transitioning from legacy branding, the application has been entirely modernized with a sleek, 2026-style interface, providing utilities with a powerful yet user-friendly tool to manage smart metering infrastructure.

## 🌟 Key Features

### 📡 Data Acquisition & Communication
- **Multi-Protocol Support**: Seamlessly communicates via DLMS, IEC, and proprietary protocols.
- **Versatile Connectivity**: Supports Serial (Optical, RS485, IrDA), FTP, GSM/GPRS, and direct CMRI (Common Meter Reading Instrument) readout.
- **Dynamic Scheduling**: Automated task managers and schedulers for bulk meter reading and data extraction.

### 📊 Comprehensive Reporting System
Powered by Crystal Reports, the application generates highly detailed, exportable reports:
- General & Nameplate Details
- Instantaneous Phasor & Measurement Reports
- Load Survey & Midnight Energies
- Tamper & Fraud Energy Logs
- Billing, Maximum Demand (MD), and Power Factor (TOD)

### 🎨 Modern 2026 UI/UX
The application underwent a complete visual overhaul to meet modern desktop standards:
- **Corporate Cabcon Branding**: Deep brand blue `RGB(29, 70, 150)` accents across the entire application.
- **Dynamic Grid Highlights**: Global row-hover interactions applied to all `DataGridViews` without compromising underlying business logic.
- **Flat & Clean Components**: Removal of legacy 3D WinForms gradients in favor of flat drop-downs, borderless buttons, and Segoe UI typography.
- **Embedded Branding**: Flawless Crystal Reports integration with natively embedded logos that scroll naturally with data.

## 🏗️ Architecture
The solution follows a strictly decoupled N-Tier architecture:
- **CAB.UI**: The WinForms presentation layer containing custom flat-styled controls (`CABForm`, `CABGridViewReadControl`).
- **CAB.BLL**: Business Logic Layer handling data processing, scheduling, and report generation workflows.
- **CAB.Entity / CABEntity**: Plain Old CLR Objects (POCOs) mapping database schemas to application state.
- **CABCommunication**: The wrapper and physical layer handling raw TCP/IP, GSM, and Serial byte streams for DLMS/IEC communication.

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2019 or higher
- .NET Framework 4.X
- SAP Crystal Reports Runtime for .NET Framework
- Appropriate Serial/Optical probe drivers for local meter communication

### Building the Project
1. Clone the repository.
2. Open the solution in Visual Studio.
3. Ensure the Crystal Reports dependencies are correctly referenced.
4. Restore any missing NuGet packages.
5. Build the solution (`Ctrl + Shift + B`) in `Debug` or `Release` mode.

## 🤝 Contributing
When contributing to this project, please adhere to the architectural boundaries:
1. **UI Modifications**: Place all visual theming inside the UI layer (e.g., `CABForm.cs` `ApplyModernTheme()` methods). Do not mix business logic with presentation.
2. **Reports**: Do not use WinForms `PictureBox` overlays for branding. Use the native Crystal Reports designer to embed dynamic `PictureObjects`.
3. **Database**: Always route data calls through the appropriate `BLL` wrapper classes.

## 📜 License
*Proprietary and Confidential.* Unauthorized copying of this project, via any medium, is strictly prohibited. Property of Cabcon.
