using System;
using System.Collections.Generic;
using System.Management;

namespace Hyperlink.Worker.ControlPanel.Services
{
    internal class SerialPortInfo
    {
        public readonly int Availability;
        public readonly string Caption;
        public readonly string ClassGuid;
        public readonly string[] CompatibleID;
        public readonly int ConfigManagerErrorCode;
        public readonly bool ConfigManagerUserConfig;
        public readonly string CreationClassName;
        public readonly string Description;
        public readonly string DeviceID;
        public readonly bool ErrorCleared;
        public readonly string ErrorDescription;
        public readonly string[] HardwareID;
        public readonly DateTime InstallDate;
        public readonly int LastErrorCode;
        public readonly string Manufacturer;
        public readonly string Name;
        public readonly string PNPClass;
        public readonly string PNPDeviceID;
        public readonly int[] PowerManagementCapabilities;
        public readonly bool PowerManagementSupported;
        public readonly bool Present;
        public readonly string Service;
        public readonly string Status;
        public readonly int StatusInfo;
        public readonly string SystemCreationClassName;
        public readonly string SystemName;

        public SerialPortInfo(ManagementBaseObject property)
        {
            var dic = new Dictionary<string, object>();
            foreach (var item in property.Properties)
            {
                dic[item.Name] = item.Value;
            }

            object getValue(string name)
            {
                dic.TryGetValue(name, out object value);
                return value;
            }

            Availability = getValue("Availability") as int? ?? 0;
            Caption = getValue("Caption") as string ?? string.Empty;
            ClassGuid = getValue("ClassGuid") as string ?? string.Empty;
            CompatibleID = getValue("CompatibleID") as string[] ?? new string[] { };
            ConfigManagerErrorCode = getValue("ConfigManagerErrorCode") as int? ?? 0;
            ConfigManagerUserConfig = getValue("ConfigManagerUserConfig") as bool? ?? false;
            CreationClassName = getValue("CreationClassName") as string ?? string.Empty;
            Description = getValue("Description") as string ?? string.Empty;
            DeviceID = getValue("DeviceID") as string ?? string.Empty;
            ErrorCleared = getValue("ErrorCleared") as bool? ?? false;
            ErrorDescription = getValue("ErrorDescription") as string ?? string.Empty;
            HardwareID = getValue("HardwareID") as string[] ?? new string[] { };
            InstallDate = getValue("InstallDate") as DateTime? ?? DateTime.MinValue;
            LastErrorCode = getValue("LastErrorCode") as int? ?? 0;
            Manufacturer = getValue("Manufacturer") as string ?? string.Empty;
            Name = getValue("Name") as string ?? string.Empty;
            PNPClass = getValue("PNPClass") as string ?? string.Empty;
            PNPDeviceID = getValue("PNPDeviceID") as string ?? string.Empty;
            PowerManagementCapabilities = getValue("PowerManagementCapabilities") as int[] ?? new int[] { };
            PowerManagementSupported = getValue("PowerManagementSupported") as bool? ?? false;
            Present = getValue("Present") as bool? ?? false;
            Service = getValue("Service") as string ?? string.Empty;
            Status = getValue("Status") as string ?? string.Empty;
            StatusInfo = getValue("StatusInfo") as int? ?? 0;
            SystemCreationClassName = getValue("SystemCreationClassName") as string ?? string.Empty;
            SystemName = getValue("SystemName") as string ?? string.Empty;
        }
    }
}