using Newtonsoft.Json;
using Roulette.Redis.Common;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Redis.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
        public async Task<string> SetListCacheAsync()
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            string Roulette = await db.StringGetAsync("NumRuleta");
            int numRoulette = 0;
            if (Roulette is null || Roulette == "")
            {
                await db.StringSetAsync("NumRuleta", 1);
                numRoulette = 1;
            }
            else
            {
                numRoulette = int.Parse(Roulette) + 1;
            }
            var data = new HashEntry[] {
                new HashEntry("Roulette"+numRoulette,0)
            };
            await db.HashSetAsync("Roulette", data);

            return "Roulette" + numRoulette;
        }
        public async Task<string> SetActivateRouletteCacheAsync(string roulette)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            string exist = await db.HashGetAsync("Roulette", roulette);
            if (exist is null || exist == "" || exist == "1")
            {
                return "No existe o ya esta activa la ruleta!";
            }
            var data = new HashEntry[] {
                new HashEntry(roulette,1)
            };
            await db.HashSetAsync("Roulette", data);

            return "true";
        }
        public async Task<string> SetBetCacheAsync(BetType bet)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            string betJson = JsonConvert.SerializeObject(bet);
            string Roulette = await db.HashGetAsync("Roulette", bet.roulette);
            if (Roulette is null || Roulette == "" || Roulette == "0")
            {
                return "Verifique el estado de la ruleta";
            }
            else
            {
                var data = new HashEntry[] {
                     new HashEntry(bet.idUser,betJson)
                };
                await db.HashSetAsync("Bet-" + bet.roulette, data);
            }

            return "True";
        }
        public async Task<string> SetCloseRouletteCacheAsync(String roulette)
        {
            Random r = new Random();
            IDatabase db = _connectionMultiplexer.GetDatabase();
            string exist = await db.HashGetAsync("Roulette", roulette);
            string numberWinner = r.Next(0, 37).ToString();
            string colorGanador;
            if (int.Parse(numberWinner) % 2 == 0)
                colorGanador = "ROJO";
            else
                colorGanador = "NEGRO";
            if (exist is null || exist == "" || exist == "0")
            {
                return "No existe o ya esta inactiva la ruleta!";
            }
            var data = new HashEntry[] {
                new HashEntry(roulette,0)
            };
            var x = await db.HashGetAllAsync("Bet-" + roulette);
            List<WinnersType> winnersList = new List<WinnersType>();
            BetType apuestas = new BetType();
            WinnersType winnersType = new WinnersType();
            foreach (var item in x)
            {
                apuestas = JsonConvert.DeserializeObject<BetType>(item.Value);
                if (apuestas.number is not null)
                {
                    if (apuestas.number == numberWinner)
                    {
                        winnersType.idUser = apuestas.idUser;
                        winnersType.valorBet = apuestas.valorBet;
                        winnersType.valorWinner = (int.Parse(apuestas.valorBet) * 5).ToString();
                        winnersType.number = apuestas.number;
                        winnersType.numberWinner = numberWinner;
                        winnersType.colorWinner = colorGanador;
                        winnersType.state = "Ganador";
                        winnersList.Add(winnersType);
                    }
                    else
                    {
                        winnersType.idUser = apuestas.idUser;
                        winnersType.valorBet = apuestas.valorBet;
                        winnersType.valorWinner = "0";
                        winnersType.color = apuestas.color;
                        winnersType.numberWinner = numberWinner;
                        winnersType.colorWinner = colorGanador;
                        winnersType.state = "Perdedor";
                        winnersList.Add(winnersType);
                    }
                }
                else if (apuestas.color is not null)
                {
                    if (apuestas.color.ToUpper() == colorGanador)
                    {
                        winnersType.idUser = apuestas.idUser;
                        winnersType.valorBet = apuestas.valorBet;
                        winnersType.valorWinner = (int.Parse(apuestas.valorBet) * 1.8).ToString();
                        winnersType.color = apuestas.color;
                        winnersType.numberWinner = numberWinner;
                        winnersType.colorWinner = colorGanador;
                        winnersType.state = "Ganador";
                        winnersList.Add(winnersType);
                    }
                    else
                    {
                        winnersType.idUser = apuestas.idUser;
                        winnersType.valorBet = apuestas.valorBet;
                        winnersType.valorWinner = "0";
                        winnersType.color = apuestas.color;
                        winnersType.numberWinner = numberWinner;
                        winnersType.colorWinner = colorGanador;
                        winnersType.state = "Perdedor";
                        winnersList.Add(winnersType);
                    }
                }
            }

            return JsonConvert.SerializeObject(winnersType);
        }
    }
}
