namespace BonetIDE
{
    public class CharacterReading
    {
        public string Character { get; private set; }
        public string Reading { get; private set; }

        public CharacterReading(string character, string reading)
        {
            Character = character;
            Reading = reading;
        }

        public override string ToString()
        {
            return $"{Character} {Reading}";
        }
    }
}