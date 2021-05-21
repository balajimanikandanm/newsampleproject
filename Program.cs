using System;
using System.Data.SqlClient;

namespace StudentQry
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Enter Student Id: ");
                string strStudentId = Console.ReadLine();

                // ******************************************************
                // Validation
                // Check StudentId is not string, > 0 && < 999
                // ******************************************************
                //int studentId = Int32.Parse(strStudentId);
                int studentId;
                bool res = Int32.TryParse(strStudentId, out studentId);
                if (!res)  //res == false  //res == true
                {
                    Console.WriteLine("Invalid Input. Please input a number");
                    return;
                }

                // Check if StudentId > 0
                if (studentId <= 0)
                {
                    Console.WriteLine("Student Id should be greater than Zero");
                    return;
                }

                if (studentId > 999)
                {
                    Console.WriteLine("Student Id should be less than 999");
                    return;
                }

                // ******************************************************
                // Do Something (Execution)
                // ******************************************************

                Student student = GetStudent(studentId);
                if(student == null)
                {
                    Console.WriteLine("Student with " + studentId + " does not exist.");
                    Console.WriteLine("Student with {0} does not exist.", studentId);
                    Console.WriteLine($"Student with {studentId} does not exist.");
                    return;
                }

                // Post Data Get
                Console.WriteLine("{0} {1} {2} {3}",
                                student.StudentId, student.Name, student.Age,
                                student.DOB.ToString("dd-MMM-yy"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Error - {0}", ex.Message);
            }
        }

        //Get Student
        static Student GetStudent(int studentId)
        {
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=SchoolDatabase;Integrated Security=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                //
                // Open the SqlConnection.
                //
                con.Open();
                //
                // This code uses an SqlCommand based on the SqlConnection.
                //
                string sqlStmt = String.Format("Select StudentId, Name, Age, DOB from Students Where StudentId ={0}", studentId);
                //string sqlStmt1 = "Select StudentId, Name, Gender, DOB from Student Where StudentId = " + studentId;
                //string sqlStmt2 = $"Select StudentId, Name, Gender, DOB from Student Where StudentId = {studentId}";
                Console.WriteLine(sqlStmt);
                //Console.WriteLine(sqlStmt1);
                //Console.WriteLine(sqlStmt2);

                using (SqlCommand command = new SqlCommand(sqlStmt, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Student s = new Student();
                            s.StudentId = reader.GetInt32(0);
                            s.Name = reader.GetString(1);
                            s.Age = reader.GetInt32(2);
                            s.DOB = reader.GetDateTime(3);

                            // Console.WriteLine("before new");
                            // Student s = new Student(reader.GetInt32(0),
                            //                 reader.GetString(1),
                            //                 reader.GetChar(2),
                            //                 reader.GetDateTime(3));

                            Console.WriteLine(s.Name);
                            return s;
                        }
                    }
                }
                return null;
            }

        }
    }
}
