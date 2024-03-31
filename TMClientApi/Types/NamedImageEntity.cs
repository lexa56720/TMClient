using System.ComponentModel;

namespace ClientApiWrapper.Types
{
    public abstract class NamedImageEntity:INotifyPropertyChanged
    {
        public virtual int Id { get; protected set; }

        public virtual string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string name = string.Empty;

        public virtual bool IsHaveImage
        {
            get => isHaveImage;
            set
            {
                isHaveImage = value;
                OnPropertyChanged(nameof(IsHaveImage));
            }
        }
        private bool isHaveImage;

        public virtual string? ImageLarge
        {
            get => imageLarge;
            set
            {
                imageLarge = value;
                OnPropertyChanged(nameof(ImageLarge));
            }
        }
        private string? imageLarge;

        public virtual string? ImageMedium
        {
            get => imageMedium;
            set
            {
                imageMedium = value;
                OnPropertyChanged(nameof(ImageMedium));
            }
        }
        private string? imageMedium;

        public virtual string? ImageSmall
        {
            get => imageSmall;
            set
            {
                imageSmall = value;
                OnPropertyChanged(nameof(ImageSmall));
            }
        }
        private string? imageSmall;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
