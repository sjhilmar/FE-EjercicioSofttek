using FE_EjercicioSofttek.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FE_EjercicioSofttek.Services
{
    public class VentasServiceImpl : IVentasService
    {
        private static int _id;
        private static string _usuario;
        private static string _contrasenia;
        private static string _baseUrl;
        private static string _token;

        public VentasServiceImpl()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _id = Convert.ToInt32(builder.GetSection("ApiSettings:id").Value);
            _usuario = builder.GetSection("ApiSettings:usuario").Value;
            _contrasenia = builder.GetSection("ApiSettings:contrasenia").Value;
            _baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;
        }

        public async Task Autenticar()
        {

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseUrl);
            var credenciales = new Usuario() { usuario = _usuario, Id = _id , contrasenia=_contrasenia};
            var content = new StringContent(JsonConvert.SerializeObject(credenciales), Encoding.UTF8, "application/json");
            var response = await cliente.PostAsync("api/Token", content);
            var json_respuesta = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<RespuestaToken>(json_respuesta);
            _token = resultado.result.ToString();
        }

        public async Task<List<Ventas>> Listar()
        {
            List<Ventas> lista = new List<Ventas>();
            await Autenticar();
            var cliente = new HttpClient();

            cliente.BaseAddress = new Uri(_baseUrl);
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            var response = await cliente.GetAsync("/api/Venta");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<Ventas>>(json_respuesta);
                lista = resultado;
            }
            return lista;

        }
        public async Task<Ventas> Obtener(int id)
        {
            Ventas venta = new Ventas();
            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseUrl);
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            var response = await cliente.GetAsync($"api/Venta/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<Ventas>(json_respuesta);
                venta = resultado;
            }
            return venta;
        }
        public async Task<bool> Guardar(Ventas ventas)
        {
            bool respuesta = false;

            await Autenticar();

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseUrl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var content = new StringContent(JsonConvert.SerializeObject(ventas), Encoding.UTF8, "application/json");

            var response = await cliente.PostAsync("api/Venta/", content);

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }

            return respuesta;
        }
        public async Task<bool> Editar(Ventas ventas,int id)
        {
            bool respuesta = false;

            await Autenticar();


            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseUrl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var content = new StringContent(JsonConvert.SerializeObject(ventas), Encoding.UTF8, "application/json");

            var response = await cliente.PutAsync($"api/Venta/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }

            return respuesta;
        }
        public async Task<bool> Eliminar(int id)
        {
            bool respuesta = false;

            await Autenticar();


            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseUrl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);


            var response = await cliente.DeleteAsync($"api/Venta/{id}");

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }

            return respuesta;
        }
    }
}
