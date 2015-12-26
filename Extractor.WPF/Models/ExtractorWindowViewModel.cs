using Extractor.Models;
using Innouvous.Utils.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private ExtractorWindow window;
        
        public ExtractorWindowViewModel(ExtractorWindow window)
        {
            this.window = window;

        }

        #region Properties
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
            }
        }

        #region State
        private bool canSelectInput = false;
        public bool CanSelectInput
        {
            get
            {
                return canSelectInput;
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
                return canSelectExtractor;
            }
            set
            {
                CanSelectExtractor = value;
                RaisePropertyChanged("CanSelectExtractor");
                RaisePropertyChanged("ValidInputs");
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
 
        public bool DetailsButtonEnabled
        {
            get
            {
                return SelectedExtractor != null;
            }
        }

        #endregion
        
        #endregion

    }
}
