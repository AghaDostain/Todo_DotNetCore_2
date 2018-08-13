//using System;
//using Microsoft.EntityFrameworkCore;
//using Todo.Common.Exceptions;
//using Todo.Exceptions;
//namespace Todo.Repositories
//{
//    internal class ExceptionHelper
//    {
//        public static void Process(DbUpdateException exception)
//        {
//            EntitySqlException sqlException = GetSqlException(exception);
//            if (sqlException != null)
//            {
//                //EntitySqlException.Number
//                switch (sqlException.HResult)
//                {
//                    case 2627:  // Unique constraint error
//                    case 547:   // Constraint check violation
//                    case 2601:  // Duplicated key row error
//                        throw new DuplicateRecordException("Duplicate data is found", sqlException);   // A custom exception of yours for concurrency issues
//                    default:
//                        // A custom exception of yours for other DB issues
//                        throw new BaseException(exception.Message, exception.InnerException);
//                }
//            }
//            throw exception;
//        }
//        private static EntitySqlException GetSqlException(Exception ex)
//        {
//            if (ex is EntitySqlException)
//            {
//                return ex as EntitySqlException;
//            }
//            if (ex.InnerException != null)
//            {
//                return GetSqlException(ex.InnerException);
//            }
//            return null;
//        }
//    }
//}