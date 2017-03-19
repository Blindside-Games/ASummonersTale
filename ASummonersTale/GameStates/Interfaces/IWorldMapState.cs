using ASummonersTale.TileEngine;

namespace ASummonersTale.GameStates.Interfaces
{
    interface IWorldMapState : IGameState
    {
        void ConstructMap(TileMap tileMap, Camera camera);
    }
}
