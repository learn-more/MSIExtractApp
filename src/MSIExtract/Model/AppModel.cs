﻿// Copyright (c) William Kent. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using MSIExtract.Msi;

namespace MSIExtract
{
    /// <summary>
    /// Provides the model backing the <see cref="MainWindow"/>.
    /// </summary>
    public sealed class AppModel : INotifyPropertyChanged
    {
        private string? msiPath;

        /// <summary>
        /// Raised when a property on this class is changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the absolute path to the MSI file being read.
        /// </summary>
        public string? MsiPath
        {
            get
            {
                return msiPath;
            }

            set
            {
                msiPath = value;

                Files.Clear();
                if (msiPath != null)
                {
                    MsiFile[] msiFiles = MsiFile.CreateMsiFilesFromMSI(new LessIO.Path(msiPath));
                    Files = new ObservableCollection<MsiFile>(msiFiles);
                }

                OnPropertyChanged(nameof(MsiPath));
                OnPropertyChanged(nameof(Files));
            }
        }

        /// <summary>
        /// Gets a collection of the files installed by the MSI.
        /// </summary>
        public ObservableCollection<MsiFile> Files { get; private set; } = new ObservableCollection<MsiFile>();

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
