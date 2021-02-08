using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Roulette.Redis.Common;
using Roulette.Redis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Roulette.Redis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        [HttpPost]
        [Route("createRoulette")]
        public async Task<IActionResult> CreateRoulette()
        {
            ResponseType response = new ResponseType();
            string keyRoulette = await _cacheService.SetListCacheAsync();
            if (keyRoulette.Length!=0)
            {
                response.Code = 200;
                response.Message = "Se ha creado correctamente "+ keyRoulette ;
                response.OutPut = JsonConvert.SerializeObject("Id: " + keyRoulette);
            }
            else
            {
                response.Code = 500;
                response.Message = "Ha ocurrido un error, verifiqué la información";
            }
            JsonConvert.SerializeObject(response);

            return Ok(JsonConvert.SerializeObject(response));
        }
        [HttpPost]
        [Route("activateRoulette")]
        public async Task<IActionResult> ActivateRoulette(string nameRoulette)
        {
            ResponseType response = new ResponseType();
            string responseActivate = await _cacheService.SetActivateRouletteCacheAsync(nameRoulette);
            if (responseActivate == "true")
            {
                response.Code = 200;
                response.Message = "Se ha activado correctamente "+ nameRoulette;
                response.OutPut = JsonConvert.SerializeObject(nameRoulette);
            }
            else
            {
                response.Code = 500;
                response.Message = responseActivate;
            }
            string outPut = JsonConvert.SerializeObject(response);

            return Ok(outPut);
        }
        [HttpPost]
        [Route("BetRoulette")]
        public async Task<IActionResult> BetRoulette(BetType bet)
        {
            ResponseType response = new ResponseType();
            if(bet.number is not null  && bet.color is not null){
                response.Code = 500;
                response.Message = "Verifique su apuesta, ya que no puede apostar a color y numero. Elija uno! ";
                response.OutPut = JsonConvert.SerializeObject(bet);

                return Ok(response);
            }
            if (bet.number is not null){
                if (int.Parse(bet.number) < 0 || int.Parse(bet.number) > 36)
                {
                    response.Code = 500;
                    response.Message = "El numero al que aposto no existe. Recuerde que se puede apostar a los numero sdel 1 al 36!";
                    response.OutPut = JsonConvert.SerializeObject(bet);

                    return Ok(response);
                }
            }
            if (bet.color is not null){
                if(bet.color.ToUpper() != "NEGRO" && bet.color.ToUpper() != "ROJO"){
                    response.Code = 500;
                    response.Message = "El color al que aposto no existe. Recuerde que los colores son ROJO o NEGRO";
                    response.OutPut = JsonConvert.SerializeObject(bet);

                    return Ok(response);
                }
            }
            if (bet.valorBet is not null) {
                if(int.Parse(bet.valorBet) > 10000){
                    response.Code = 500;
                    response.Message = "El valor máximo a apostar es de $10.000 dólares";
                    response.OutPut = JsonConvert.SerializeObject(bet);

                    return Ok(response);
                }
            } 
            string responseActivate = await _cacheService.SetBetCacheAsync(bet);
            if (responseActivate == "True"){
                response.Code = 200;
                response.Message = "Se ha apostado a la ruleta " + bet.roulette;
                response.OutPut = JsonConvert.SerializeObject(bet);
            }
            else{
                response.Code = 500;
                response.Message = responseActivate;
            }

            return Ok(response);
        }
        [HttpPost]
        [Route("closeRoulette")]
        public async Task<IActionResult> CloseRoulette(string nameRoulette)
        {
            ResponseType response = new ResponseType();
            string responseClose = await _cacheService.SetCloseRouletteCacheAsync(nameRoulette);
            if (responseClose is not null)
            {
                response.Code = 200;
                response.Message = "Se ha cerrado la mesa. A continuación los resultados ";
                response.OutPut = responseClose;
            }
            else
            {
                response.Code = 500;
                response.Message = "No existe o ya esta inactiva la ruleta!";
            }
            string outPut = JsonConvert.SerializeObject(response); 

            return Ok(outPut);
        }
    }
}
