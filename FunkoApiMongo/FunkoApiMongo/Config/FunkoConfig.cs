namespace FunkoApiMongo.Config;

public class FunkoConfig {
    public string ConnectionString {  get; set; }
    public string DatabaseName { get; set; }
    public string Name { get; set; } = "Funko API MongoDB";
    public string Cache { get; set; } = "Funko API Cache";
}