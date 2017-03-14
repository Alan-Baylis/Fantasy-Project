using System.Collections.Generic;

public class Chunk : WorldObject {

    //The number of blocks per length and width of the chunk,
    //this number squared is the number of columns per chunk
    public static int chunkSize = 16;

    private ChunkLocation location;
    private Biome biome;

    private bool rendered = false;

    public List<Location> columnBaseLocations = new List<Location>();
    public Dictionary<Location, ChunkColumn> columns = new Dictionary<Location, ChunkColumn>();

    public void initialise(ChunkLocation location, Biome biome) {

        base.initialise(location);
        this.biome = biome;
        transform.position = location.asTransform();
    }

    private void Start() {
    }

    public void toggleRender() {

        foreach(ChunkColumn chunkColumn in getColumns()) {
            chunkColumn.toggleRender();
        }
        this.rendered = !isRendered();
        transform.position = getLocation().asTransform();

    }

    public Biome getBiome() {
        return this.biome;
    }

    public bool isRendered() {
        return this.rendered;
    }

    public ChunkColumn[] getColumns() {

        ChunkColumn[] chunkColumns = new ChunkColumn[Chunk.chunkSize * Chunk.chunkSize];

        int i = 0;
        foreach(ChunkColumn chunkColumn in columns.Values) {
            chunkColumns[i] = chunkColumn;
            i++;
        }

        return chunkColumns;

    }

    public void setColumn(Location location, ChunkColumn chunkColumn) {

        //Remove the old entry if there is one and then add the new entry
        if(columns.ContainsKey(location)) {
            columns.Remove(location);
            columnBaseLocations.Remove(location);
        }

        columns.Add(location, chunkColumn);
        columnBaseLocations.Add(location);

    }

    public ChunkColumn getColumn(Location location) {

        if(columns.ContainsKey(location)) {
            return columns[location];
        } else {
            return null;
        }

    }

}
