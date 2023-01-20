using FE_EjercicioSofttek.Models;
using Newtonsoft.Json;
using System.Text;

namespace FE_EjercicioSofttek.Services
{
    public class VentasServiceImpl : IVentasService
    {
        private static int _id;
        private static string _descripcion;
        private static string _baseUrl;
        private static string _token;

        public VentasServiceImpl()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _id = Convert.ToInt32(builder.GetSection("ApiSetting:id").Value);
            _descripcion = builder.GetSection("ApiSetting:descripcion").Value;
            _baseUrl = builder.GetSection("ApiSetting:baseUrl").Value;
        }

        public async Task Auntenticar()
        {

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseUrl);
            var credenciales = new AsesorComercial() { descripcion = _descripcion, Id = _id };
            var content = new StringContent(JsonConvert.SerializeObject(credenciales), Encoding.UTF8, "application/json");
            var response = await cliente.PostAsync("api/Token", content);
            var json_respuesta = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<RespuestaToken>(json_respuesta);
            _token = resultado.result.ToString();
        }

        public Task<bool> Editar(Ventas ventas)
        {
            throw new NotImplementedException();
        }
        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Guardar(Ventas ventas)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Ventas>> Listar()
        {
            List<Ventas> lista = new List<Ventas>();
            await Auntenticar();
            var cliente = new HttpClient();

            cliente.BaseAddress = new Uri(_baseUrl);
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            var response = await cliente.GetAsync("/api/Ventas");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<Ventas>(json_respuesta);
                lista.Add(resultado);
            }
            return lista;

        }

        public Task<Ventas> Obtener(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<Ventas>> IVentasService.Listar()
        {
            throw new NotImplementedException();
        }

        Task<Ventas> IVentasService.Obtener(int id)
        {
            throw new NotImplementedException();
        }
    }
}
