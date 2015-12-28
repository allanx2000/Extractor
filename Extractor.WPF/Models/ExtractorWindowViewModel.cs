using Extractor.Models;
using Innouvous.Utils;
using Innouvous.Utils.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Extractor.WPF.Models
{
    class ExtractorWindowViewModel : ViewModel
    {
        #region Static
        private static readonly Dictionary<string, AbstractExtractor> extractors;

        static ExtractorWindowViewModel()
        {
            extractors = new Dictionary<string, AbstractExtractor>()
            {
                { "TED Videos", new TEDExtractor.TEDExtractor() },
                { "SlideShare Favorites", new SlideShareLike.SlideShareLikeExtractor() },

            };
        }
        #endregion

        private Window window;
        OptionsControl optionsControl;

        private bool isSet = false;

        public ExtractorWindowViewModel(OptionsControl optionsControl, Window window)
        {
            this.window = window;
            this.optionsControl = optionsControl;
        }

        private void RefreshInputs()
        {
            CanSelectInput = selectedExtractor != null;
            RaisePropertyChanged("ValidInputs");

            if (selectedExtractor != null)
            {
                SelectedInput = extractors[selectedExtractor].ValidTypes.First();
            }
            else
                SelectedInput = null;

            RaisePropertyChanged("SelectedInput");
        }


        #region Select Extractor

        #region Inputs
        public ICollection<string> Extractors
        {
            get
            {
                return extractors.Keys;
            }
        }

        private string selectedExtractor;
        public string SelectedExtractor
        {
            get
            {
                return selectedExtractor;
            }
            set
            {
                selectedExtractor = value;
                RaisePropertyChanged("SelectedExtractor");
                RaisePropertyChanged("DetailsButtonEnabled");

                RefreshInputs();
            }
        }

        public ICollection<DataType> ValidInputs
        {
            get
            {
                if (SelectedExtractor == null)
                    return null;
                else
                {
                    var extractor = extractors[SelectedExtractor];
                    return extractor.ValidTypes;
                }
            }
        }

        public DataType? SelectedInput { get; set; }

        #endregion

        #region Enabled
        private bool canSelectInput = false;
        public bool CanSelectInput
        {
            get
            {
                return !isSet && canSelectInput;
            }
            set
            {
                canSelectInput = value;
                RaisePropertyChanged("CanSelectInput");
            }
        }

        private bool canSelectExtractor = true;
        public bool CanSelectExtractor
        {
            get
            {
                return !isSet && canSelectExtractor;
            }
            set
            {
                canSelectExtractor = value;
                RaisePropertyChanged("CanSelectExtractor");
            }
        }

        public bool DetailsButtonEnabled
        {
            get
            {
                return SelectedExtractor != null;
            }
        }

        #endregion

        #region Set Button
        public string SetText
        {
            get
            {
                return isSet ? "Unset" : "Set";
            }
        }

        public ICommand ToggleSetCommand
        {
            get
            {
                return new CommandHelper(ToggleSet);
            }
        }

        private void ToggleSet()
        {
            if (SelectedExtractor == null || SelectedInput == null)
                MessageBoxFactory.ShowError("Extractor parameters are not set");
            else
            {
                isSet = !isSet;

                RefreshViewModel();
            }
        }
        #endregion

        #endregion

        #region Input Path Section

        public ICommand BrowsePathCommand
        {
            get
            {
                return new CommandHelper(BrowseForPath);
            }
        }

        private void BrowseForPath()
        {
            if (!SelectedInput.HasValue)
                return;

            if (SelectedInput == DataType.FilePath)
            {
                var openFile = DialogsUtility.CreateOpenFileDialog(followLinks: false);
                openFile.ShowDialog();

                if (!String.IsNullOrEmpty(openFile.FileName))
                    InputPath = openFile.FileName;
            }
            else if (SelectedInput == DataType.FolderPath)
            {
                var openFolder = DialogsUtility.CreateFolderBrowser();
                openFolder.ShowDialog();

                if (!String.IsNullOrEmpty(openFolder.SelectedPath))
                    InputPath = openFolder.SelectedPath;
            }
        }

        private string inputPath;
        public string InputPath
        {
            get
            {
                return inputPath;
            }
            set
            {
                inputPath = value;
                RaisePropertyChanged("InputPath");
            }
        }

        public Visibility InputPathVisibility
        {
            get
            {
                if (isSet)
                {
                    switch (SelectedInput)
                    {
                        case DataType.FilePath:
                        case DataType.FolderPath:
                            return Visibility.Visible;
                    }
                }

                return Visibility.Collapsed;
            }
        }

        #endregion


        #region Extract

        public Visibility ExtractVisibility
        {
            get
            {
                return isSet ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public ICommand ExtractCommand
        {
            get
            {
                return new CommandHelper(Extract);
            }
        }

        private void Extract()
        {
            try
            {
                AbstractExtractor extractor = extractors[selectedExtractor];

                if (SelectedInput == null) //Not possible?
                    throw new Exception("No input is selected");

                object data;

                switch (SelectedInput.Value)
                {
                    case DataType.FilePath:
                    case DataType.FolderPath:
                        data = InputPath;
                        break;
                    default:
                        data = null;
                        break;
                }

                InputData input = new InputData(SelectedInput.Value, data);

                var items = extractor.Extract(input);

                StringBuilder sb = new StringBuilder();

                string pre = optionsControl.PrependText;
                string post = optionsControl.AppendText;

                foreach (var i in items)
                {
                    //TODO: Replace with allow tokenizer in OutputControl
                    string text = i.GetDefaultText();

                    sb.AppendLine(pre + text + post);
                }

                OutputText = sb.ToString();
            }
            catch (Exception e)
            {
                MessageBoxFactory.ShowError(e.Message, "Error Extracting Data");
            }
        }

        #endregion

        #region Options

        public Visibility OptionsVisibility
        {
            get
            {
                return isSet ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        #endregion

        #region Output

        public Visibility OutputVisibility
        {
            get
            {
                return isSet ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private string outputText;
        public string OutputText
        {
            get
            {
                return outputText;
            }
            set
            {
                outputText = value;
                RaisePropertyChanged("OutputText");
            }
        }
        #endregion


    }
}
