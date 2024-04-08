using System.ComponentModel;

namespace ClientApiWrapper.Types
{
    public abstract class NamedImageEntity : INotifyPropertyChanged
    {
        public virtual int Id { get; private set; }

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
        private string? imageLarge = null;

        public virtual string? ImageMedium
        {
            get => imageMedium;
            set
            {
                imageMedium = value;
                OnPropertyChanged(nameof(ImageMedium));
            }
        }
        private string? imageMedium = null;

        public virtual string? ImageSmall
        {
            get => imageSmall;
            set
            {
                imageSmall = value;
                OnPropertyChanged(nameof(ImageSmall));
            }
        }
        private string? imageSmall=null;

        public NamedImageEntity(int id, string name, string? picLarge, string? picMedium, string? picSmall)
        {
            Id = id;
            Name = name;

            if (picLarge == null || picMedium == null || picSmall == null)
                IsHaveImage = false;
            else
            {
                IsHaveImage = true;

                ImageLarge = picLarge;
                ImageMedium = picMedium;
                ImageSmall = picSmall;
            }
        }
        public NamedImageEntity(int id, string name)
        {
            Id = id;
            Name = name;
            IsHaveImage = false;
        }
        protected NamedImageEntity()
        {
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void Update(NamedImageEntity entity)
        {
            if (!entity.Name.Equals(Name))
                Name = entity.Name;

            if (!IsStringsEquals(ImageLarge, entity.ImageLarge))
                ImageLarge = entity.ImageLarge;

            if (!IsStringsEquals(ImageMedium, entity.ImageMedium))
                ImageMedium = entity.ImageMedium;

            if (!IsStringsEquals(ImageSmall, entity.ImageSmall))
                ImageSmall = entity.ImageSmall;

            if (IsHaveImage != entity.IsHaveImage)
                IsHaveImage = entity.IsHaveImage;
        }

        private bool IsStringsEquals(string? a, string? b)
        {
            return a == b || (a != null && a.Equals(b)) || (b != null && b.Equals(a));
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
