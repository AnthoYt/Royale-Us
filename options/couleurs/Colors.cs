colors.Add(new CustomColor { longname = "Slime", color = new Color32(244, 255, 188, byte.MaxValue), shadow = new Color32(167, 239, 112, byte.MaxValue), isLighterColor = false });
colors.Add(new CustomColor { longname = "Navy", color = new Color32(9, 43, 119, byte.MaxValue), shadow = new Color32(0, 13, 56, byte.MaxValue), isLighterColor = false });
colors.Add(new CustomColor { longname = "Darkness", color = new Color32(36, 39, 40, byte.MaxValue), shadow = new Color32(10, 10, 10, byte.MaxValue), isLighterColor = false });
colors.Add(new CustomColor { longname = "Ocean", color = new Color32(55, 159, 218, byte.MaxValue), shadow = new Color32(62, 92, 158, byte.MaxValue), isLighterColor = false });
colors.Add(new CustomColor { longname = "Sundown", color = new Color32(252, 194, 100, byte.MaxValue), shadow = new Color32(197, 98, 54, byte.MaxValue), isLighterColor = false });
pickableColors += (uint)colors.Count;

int id = 50000;
foreach (CustomColor cc in colors) {
    longlist.Add((StringNames)id);
    CustomColors.ColorStrings[id++] = cc.longname;
    colorlist.Add(cc.color);
    shadowlist.Add(cc.shadow);
    if (cc.isLighterColor)
        lighterColors.Add(colorlist.Count - 1);
}

Palette.ColorNames = longlist.ToArray();
Palette.PlayerColors = colorlist.ToArray();
Palette.ShadowColors = shadowlist.ToArray();

protected internal struct CustomColor {
    public string longname;
    public Color32 color;
    public Color32 shadow;
    public bool isLighterColor;
}
