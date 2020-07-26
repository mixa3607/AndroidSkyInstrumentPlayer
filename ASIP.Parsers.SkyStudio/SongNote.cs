using System;

namespace ASIP.Parsers.SkyStudio
{
    public class SongNote
    {
        private string _key;
        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                var ids = _key.Split("Key");
                if (ids.Length != 2)
                {
                    throw new ArgumentException($"Not valid key ({_key})");
                }

                HandNum = int.Parse(ids[0]);
                KeyId = int.Parse(ids[1]);
            }
        }

        public int Time { get; set; }

        public int KeyId { get; private set; }
        public int HandNum { get; private set; }

        public override string ToString()
        {
            return $"Hand:{HandNum}, KeyId: {KeyId}, Time: {Time}";
        }
    }
}
