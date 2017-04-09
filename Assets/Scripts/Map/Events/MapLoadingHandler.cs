
//This Event Handler loads chunks around the player as they move so the surrounding map is always loaded around them
using System.Collections;
using UnityEngine;

public class MapLoadingHandler : IListener {

    public static int loadedChunksRange = 2;
    public static int renderedChunksRange = 1;

    private GameManager gameManager;

    //This is just an example event handler template it hows how the event listener method must be set out
    public MapLoadingHandler(GameManager gameManager) {
        this.gameManager = gameManager;
    }

    //The concept is simple, render a border of one chunk wide around the player, but load a 2 wide border of chunks around the player
    [EventListener]
    public void onPlayerMove(PlayerMoveEvent ev) {

        Location location = ev.getLocation();
        World world = location.getWorld();

        Vector3 movement = ev.getMovement();

        Direction moveDirection = DirectionMethods.getDominantDirection(movement);
        Location ordinalMovement = moveDirection.ordinal(world) * Chunk.chunkSize;

        ChunkLocation playerChunkLocation = ChunkLocation.asChunkLocation(location + ordinalMovement);

        loadChunksInRange(playerChunkLocation);

        unloadChunksOutOfRange(playerChunkLocation);

    }

    private void unloadChunksOutOfRange(ChunkLocation centre) {

        World world = centre.getWorld();

        foreach(Chunk chunk in world.getLoadedChunks()) {
            Location chunksLocation = chunk.getLocation();
            Debug.Log(centre.distance(chunksLocation));
            if(centre.distance(chunksLocation) > (loadedChunksRange + 0.5) * Chunk.chunkSize) {
                Debug.Log("=> Boop");
                world.destroyChunk(chunk);
            }
        }
    }

    private void loadChunksInRange(ChunkLocation centre) {

        World world = centre.getWorld();

        for(int x = -loadedChunksRange; x < loadedChunksRange; x++) {
            for(int y = -loadedChunksRange; y < loadedChunksRange; y++) {
                ChunkLocation chunkLocation = centre + new ChunkLocation(world, x, y, 0);

                if(!world.containsChunk(chunkLocation)) {
                    world.loadChunk(new TerrainGenerator(gameManager, world, new Hills()), chunkLocation);
                }
            }
        }

        for(int x = -renderedChunksRange; x < renderedChunksRange; x++) {
            for(int y = -renderedChunksRange; y < renderedChunksRange; y++) {
                ChunkLocation chunkLocation = centre + new ChunkLocation(world, x, y, 0);

                if(world.containsChunk(chunkLocation)) {
                    Chunk chunk = world.getChunk(chunkLocation);
                    if(!chunk.isRendered()) {
                        chunk.toggleRender();
                    }
                }
            }
        }

    }

}
