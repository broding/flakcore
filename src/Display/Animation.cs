struct Animation
{
    public string name;
    public int[] frames;
    public float frameRate;

    public Animation(string name, int[] frames, float frameRate)
    {
        this.name = name;
        this.frames = frames;
        this.frameRate = frameRate;
    }
}