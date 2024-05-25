using Application.DTO.Response;
using MediatR.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public static class GeneralDbResponses
    {
        public static ServiceResponse ItemAlreadyExist(string itemName) => new(false, $"{itemName} already exist");
        public static ServiceResponse ItemNotFound(string itemName) => new(false, $"{itemName} not found");
        public static ServiceResponse ItemCreated(string itemName) => new(true, $"{itemName} successfully created");
        public static ServiceResponse ItemUpdated(string itemName) => new(true, $"{itemName} successfully updated");
        public static ServiceResponse ItemDeleted(string itemName) => new(true, $"{itemName} successfully deleted");
    }
}
