public class CustomOption {
    public enum CustomOptionType {
        Impostor,
        Crewmate,
    }

    public static List<CustomOption> options = new();
    public static int preset = 0;
    public static ConfigEntry<string> vanillaSettings;

    public int id;
    public string name;
    public string format;
    public System.Object[] selections;
    public int defaultSelection;
    public ConfigEntry<int> entry;
    public int selection;
    public OptionBehaviour optionBehaviour;
    public CustomOption parent;
    public bool isHeader;
    public CustomOptionType type;
    public string heading = "";

    public CustomOption(int id, CustomOptionType type, string name, System.Object[] selections, System.Object defaultValue, CustomOption parent, bool isHeader, string format, string heading = "") {
        ...
    }

    public static CustomOption Create(int id, CustomOptionType type, string name, string[] selections, CustomOption parent = null, bool isHeader = false, string format = "", string heading = "") {
        ...
    }

    public static CustomOption Create(int id, CustomOptionType type, string name, float defaultValue, float min, float max, float step, CustomOption parent = null, bool isHeader = false, string format = "", string heading = "") {
        ...
    }

    public static CustomOption Create(int id, CustomOptionType type, string name, bool defaultValue, CustomOption parent = null, bool isHeader = false, string format = "", string heading = "") {
        ...
    }

    public static CustomOption Create(int id, CustomOptionType type, string name, List<RoleId> roleId, CustomOption parent = null, bool isHeader = false) {
        ...
    }

    // Static behaviour

}
