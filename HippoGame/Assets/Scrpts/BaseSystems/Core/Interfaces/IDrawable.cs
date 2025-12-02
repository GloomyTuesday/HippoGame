namespace Scripts.BaseSystems.CoreInterfaces
{
    public interface IDrawable 
    {
        public bool DrawFlag { get; }
        public void Draw(bool drawFlag = true); 
    }
}

