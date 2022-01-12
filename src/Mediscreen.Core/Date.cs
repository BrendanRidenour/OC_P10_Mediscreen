namespace Mediscreen
{
    public class Date
    {
        public virtual int Year { get; set; }

        public virtual int Month { get; set; }

        public virtual int Day { get; set; }

        public Date(string value)
            : this()
        {
            try
            {
                var segments = value.Split('-');

                if (segments.Length != 3)
                    ThrowArgumentException();

                this.Year = int.Parse(segments[0]);
                this.Month = int.Parse(segments[1]);
                this.Day = int.Parse(segments[2]);
            }
            catch
            {
                ThrowArgumentException();
            }

            static void ThrowArgumentException()
            {
                throw new ArgumentException(paramName: nameof(value), message: "Value could not be parsed.");
            }
        }

        public Date(int year, int month, int day)
            : this()
        {
            _ = new DateTime(year, month, day);

            this.Year = year;
            this.Month = month;
            this.Day = day;
        }

        public Date() { }

        public override string ToString() => $"{Year:0000}-{Month:00}-{Day:00}";
    }
}