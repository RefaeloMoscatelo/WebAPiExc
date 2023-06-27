using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class PersonManager
    {
        private Database db;
        public PersonManager()
        {
            db = new Database("rafaeldb");
        }

        public List<Person> GetAll()
        {
            List<Person> list = new List<Person>();
            SqlDataReader reader = null;
            try
            {
                db.Open();
                reader = db.ExecuteReader("select [ID],[FirstName],[LastName],[Email],[Phone] FROM [RafaelDB].[dbo].[Person]");
                while (reader.Read())
                {
                    Person tmp = new Person();
                    tmp.ID = (int)reader[0];
                    if (!reader.IsDBNull(1))
                        tmp.FirstName = (string)reader[1];
                    if (!reader.IsDBNull(2))
                        tmp.LastName = (string)reader[2];
                    if (!reader.IsDBNull(3))
                        tmp.Email = (string)reader[3];
                    if (!reader.IsDBNull(4))
                        tmp.Phone = (string)reader[4];
                    list.Add(tmp);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                db.Close();
            }

            return list;
        }

        public void Delete(int userID)
        {
            try
            {
                db.Open();

                var affected = db.ExecuteNonQuery("sp_deletePerson",true, db.CreateParameter("@id", userID.ToString()));
                if (affected == 0)
                {
                    throw new Exception("No rows affected");
                }
            }
            finally
            {
                db.Close();
            }

        }

        public void Update(Person person)
        {
            try
            {
                db.Open();
                string query = @"Update [RafaelDB].[dbo].[Person] set
                    [FirstName] = @FirstName
                    ,[LastName] = @LastName
                    ,[Email] = @Email
                    ,[Phone] = @Phone where [ID]=@id";
                var affected = db.ExecuteNonQuery(query, db.CreateParameter("@id", person.ID.ToString())
                                                        , db.CreateParameter("@FirstName", person.FirstName)
                                                        , db.CreateParameter("@LastName", person.LastName)
                                                        , db.CreateParameter("@Email", person.Email)
                                                        , db.CreateParameter("@Phone ", person.Phone));
                if (affected == 0)
                {
                    throw new Exception("No rows affected");
                }
            }
            finally
            {
                db.Close();
            }

        }
        public int Add(Person person)
        {

            var query = @"	INSERT INTO [RafaelDB].[dbo].[Person] ([FirstName],[LastName],[Email],[Phone])
		                                        VALUES(@FirstName,@LastName,@Email,@Phone);
                                                               SELECT @newID = @@IDENTITY;";
            SqlParameter newIdParameter = db.CreateParameter("@newID", 0);
            newIdParameter.Direction = System.Data.ParameterDirection.Output;
            try
            {
                db.Open();
                var affected = db.ExecuteNonQuery(query, db.CreateParameter("@FirstName", person.FirstName)
                                                        , db.CreateParameter("@LastName", person.LastName)
                                                        , db.CreateParameter("@Email", person.Email)
                                                        , db.CreateParameter("@Phone ", person.Phone)
                                                         , newIdParameter);
                if (affected == 0)
                    throw new Exception("No rows affected");



                person.ID = (int)newIdParameter.Value;

            }
            finally
            {
                db.Close();
            }
            return person.ID;
        }
        public Person GetByID(int userID)
        {
            Person p = new Person();
            try
            {
                db.Open();

                var affected = db.ExecuteNonQuery("sp_getbyID", true, db.CreateParameter("@id", userID.ToString()));
                if (affected == 0)
                {
                    throw new Exception("No rows affected");
                }
            }
            finally
            {
                db.Close();
            }
            return p;
        }

    }
}
