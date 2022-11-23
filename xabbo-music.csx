using System.Drawing;

var song = "b N b c - b N b c - w w m - q q b - N N q m N b N b c - N N q m N b N b c - w w r w m q e - q b c b v x z".ToLower().Split(' ');

var wiredSpace = new List<Point>() { new Point(4,3), new Point(9, 1) };
var effectSpace = new Point(4, 13);

var magicTileId = 1665111496;

//--------------------------

var wiredPoints = new List<Point>();

var effectMap = new Dictionary<string, Point>();

for (var i = wiredSpace[0].X; i <= wiredSpace[1].X; i++){
  for (var j = wiredSpace[0].Y; j >= wiredSpace[1].Y; j--){
    wiredPoints.Add(new Point(i, j));
  }
}

Log(wiredPoints.Count().ToString());

var bcCatalogRoot = GetBcCatalog();
ICatalogPage GetBcPageNamed(string name) => GetBcCatalogPage(bcCatalogRoot.First(x => x.Name == name).Id);

var allNotes = GetNotes();
var roomEffects = GetRoomEffects();
var bcCatalogPages = GetCatalogPages();
var bcCatalogOffers = GetCatalogOffers();


var notes = new List<List<Note>>();

foreach (var note in song){
  var noteList = new List<Note>();
  if (note == "-"){
    notes.Add(noteList);
    continue;
  }
  
  foreach (var subNote in note.ToCharArray()){
    if (allNotes.ContainsKey(subNote.ToString()))
      noteList.Add(allNotes[subNote.ToString()]);
  }
  
  notes.Add(noteList);
}

var wiredSquareIndex = 0;
var delay = 1;

foreach (var note in allNotes){
  var allNotesInSong = new List<string>();
  
  foreach (var subpart in song){
    var stringList = new List<string>();
  
    foreach (var chr in subpart.ToCharArray())
      stringList.Add(chr.ToString());
      
    allNotesInSong.AddRange(stringList);
  }
  
  if (!allNotesInSong.Contains(note.Key)) continue;  
  
  if (!roomEffects[note.Value.Furni].Any(x => x.Location.Z.ToString() == (note.Value.Height == "0" ? "0" : (float.Parse(note.Value.Height) / 100).ToString()))){
    ICatalogOffer effect = bcCatalogOffers[note.Value.Furni];
    var location = GetEffectPoint(note.Value.Furni);
    var height = int.Parse(note.Value.Height);
    
    Send(Out["SetCustomStackingHeight"], magicTileId, height);
    Delay(250);
    Send(Out["BuildersClubPlaceRoomItem"], bcCatalogPages["sfx"].Id, effect.Id, "", location.X, location.Y, 0);
    Delay(250);
    
    note.Value.PlacedEffect = true;
  }
  
  roomEffects = GetRoomEffects();
  
  note.Value.Location = wiredPoints[wiredSquareIndex];
  wiredSquareIndex++;
}

foreach (var noteList in notes){
  if (noteList.Count() == 0){
    delay++;
    continue;
  }

  foreach (var note in noteList){
    if (!note.Placed) {
      note.Placed = true;
      var soundEffect = roomEffects[note.Furni].First(x => x.Location.Z.ToString() == (note.Height == "0" ? "0" : (float.Parse(note.Height) / 100).ToString()));
      Send(Out["BuildersClubPlaceRoomItem"], bcCatalogPages["wf_act"].Id, bcCatalogOffers["wf_act_toggle_state"].Id, "", note.Location.X, note.Location.Y, 0);
      var effect = FloorItem.Parse(await ReceiveAsync(In["ObjectAdd"]));
      Delay(250);
      Send(Out["UpdateAction"], (int)effect.Id, 0, "", 1, (int)soundEffect.Id, 0, 0);
      Delay(250);
    }
  
    Send(Out["BuildersClubPlaceRoomItem"], bcCatalogPages["wf_trg"].Id, bcCatalogOffers["wf_trg_at_given_time"].Id, "", note.Location.X, note.Location.Y, 0);
    var trigger = FloorItem.Parse(await ReceiveAsync(In["ObjectAdd"]));
    Delay(250);
    Send(Out["UpdateTrigger"], (int)trigger.Id, 1, delay, 0, 0, "");
    Delay(250);
  }
  
  delay++;
}

Dictionary<string, Note> GetNotes(){
  var funkHornNotes = new string[]{ "←", "∟", "↔", "▲", "▼", "!", "#", "$", "%", "&", "(", ")", "*" };
  var xyloHighNotes = new string[]{ "q", "2", "w", "3", "e", "r", "5", "t", "6", "y", "7", "u", "i" };
  var xyloNotes = new string[]{ "z", "s", "x", "d", "c", "v", "g", "b", "h", "n", "j", "m", "," };
  var duckNotes = new string[]{ "♫", "☼", "►", "◄", "↕", "‼", "¶", "§", "▬", "↨", "↑", "↓", "→" };
  var padNotes = new string[]{ "☺", "☻", "♥", "♦", "♣", "♠", "•", "◘", "○", "◙", "♂", "♀", "♪" };
  var height = "0";
  
  var result = new Dictionary<string, Note>();

  foreach (var note in xyloHighNotes){
    result.Add(note, new Note() { Height = height, Furni = "sfx_xylo_high" });
    height = (int.Parse(height) + 50).ToString();
  }

  height = "0";

  foreach (var note in xyloNotes){
    result.Add(note, new Note() { Height = height, Furni = "sfx_xylo" });
    height = (int.Parse(height) + 50).ToString();
  }

  height = "0";

  foreach (var note in padNotes){
    result.Add(note, new Note() { Height = height, Furni = "sfx_pad1" });
    height = (int.Parse(height) + 50).ToString();
  }

  height = "0";

  foreach (var note in duckNotes){
    result.Add(note, new Note() { Height = height, Furni = "sfx_duck" });
    height = (int.Parse(height) + 50).ToString();
  }

  height = "0";

  foreach (var note in funkHornNotes){
    result.Add(note, new Note() { Height = height, Furni = "sfx_funkhorn" });
    height = (int.Parse(height) + 50).ToString();
  }
  
  return result;
}

Dictionary<string, IEnumerable<IFloorItem>> GetRoomEffects(){
  var result = new Dictionary<string, IEnumerable<IFloorItem>>();
  
  foreach (var note in allNotes.Values)
  {
    if (!result.ContainsKey(note.Furni))
    {
      result.Add(note.Furni, Room.FloorItems.Where(x => x.GetIdentifier() == note.Furni));
    }
  }
  
  return result;
}

Dictionary<string, ICatalogOffer> GetCatalogOffers()
{
  var result = new Dictionary<string, ICatalogOffer>()
  {
    { "wf_trg_at_given_time", bcCatalogPages["wf_trg"].Offers.First(x => x.Products.Any(p => p.GetIdentifier() == "wf_trg_at_given_time")) },
    { "wf_act_toggle_state", bcCatalogPages["wf_act"].Offers.First(x => x.Products.Any(p => p.GetIdentifier() == "wf_act_toggle_state")) }
  };
  
  foreach (var note in allNotes.Values)
  {
    if (!result.ContainsKey(note.Furni))
    {
      result.Add(note.Furni, bcCatalogPages["sfx"].Offers.First(x => x.Products.Any(p => p.GetIdentifier() == note.Furni)));
    }
  }
  
  return result;
}

Dictionary<string, ICatalogPage> GetCatalogPages()
{
  var result = new Dictionary<string, ICatalogPage>()
  {
    { "wf_trg", GetBcPageNamed("wired_triggers") },
    { "wf_act", GetBcPageNamed("wired_effects") },
    { "sfx", GetBcPageNamed("sound_fx") }
  };
  
  return result;
}

Point GetEffectPoint(string effectIdentifier)
{
  if (!effectMap.ContainsKey(effectIdentifier))
  {
    effectMap.Add(effectIdentifier, effectSpace);
  }
  
  return effectMap[effectIdentifier];
}

public class Note {
  public string Height { get; set; }
  public string Furni { get; set; }
  public Point Location { get; set; }
  public bool Placed { get; set; }
  public bool PlacedEffect { get; set; }
}