using ApiPersonajesAWS.Data;
using ApiPersonajesAWS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace ApiPersonajesAWS.Repositories
{
    public class RepositoryPersonajes
    {
        private PersonajesContext context;
        public RepositoryPersonajes(PersonajesContext context)
        {
            this.context = context;
        }
        public async Task<List<Personaje>>GetPersonajesAsync()
        {
            return await this.context.Personajes.ToListAsync();
        }

        public async Task<Personaje> FindPersonajeAsync(int id)
        {
            return await this.context.Personajes.FirstOrDefaultAsync(x => x.IdPersonaje == id);
        }

        private async Task<int> GetMaxIdPersonajeAsync()
        {
            return await this.context.Personajes.MaxAsync(x => x.IdPersonaje) + 1;
        }

        public async Task CreatePersonajeAsync(string nombre, string imagen)
        {
            Personaje personaje = new Personaje
            {
                IdPersonaje = await this.GetMaxIdPersonajeAsync(),
                Nombre = nombre,
                Imagen = imagen
            };
            this.context.Personajes.Add(personaje);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdatePersonajeAsync(int idpersonaje, string nombre, string imagen)
        {
            string sql = "CALL SPUPDATE_PERSONAJES (@p_id, @p_nombre, @p_imagen)";
            MySqlParameter pamId = new MySqlParameter("@p_id", idpersonaje);
            MySqlParameter pamNombre = new MySqlParameter("@p_nombre", nombre);
            MySqlParameter pamImagen = new MySqlParameter("@p_imagen", imagen);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamId, pamNombre, pamImagen);
        }

    }
}
