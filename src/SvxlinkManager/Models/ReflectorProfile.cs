namespace SvxlinkManager.Models
{
  public class ReflectorProfile : IModelEntity
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Config { get; set; }

    public bool Enable { get; set; }
  }
}
