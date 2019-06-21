using System;
using System.Collections.Generic;
using System.Linq;
using API.PassportValidator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PassportValidation;
using PassportModel = API.PassportValidator.Models.PassportModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.PassportValidator.Controllers
{
    [Produces("application/json")]
    [Route("api/PassportValidator")]
    public class PassportValidatorController : Controller
    {
        private readonly IValidatePassport _validatePassport;

        public PassportValidatorController(IValidatePassport validatePassport)
        {
            _validatePassport = validatePassport;
        }


        [HttpPost]
        [Route("ValidateMZR")]
        public IActionResult ValidateMZR([FromBody]PassportModel model)
        {
            try
            {
                ValidatePassport(model);

                var data = new Message<PassportModel>
                {
                    Data = model,
                    IsSuccess = true,
                    ReturnMessage = ""
                };

                return Ok(data);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Validation Method Error");
            }
        }

        [HttpGet]
        [Route("GetNationalities")]
        public IActionResult GetNationalities()
        {
            try
            {
                var nationCodes = _validatePassport.GetNations();
                List<Nationalities> list = nationCodes
                    .Select(x => new Nationalities { Alpha3Code = x.Alpha3Code, Name = x.Name }).ToList();

                return Ok(list);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Unable to return requested resource");
            }
        }

        private void ValidatePassport(PassportModel model)
        {
            var passport = new PassportValidation.Models.PassportModel
            {
                PassportNumber = model.PassportNumber,
                Nationality = model.Nationality,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                DateOfExpiration = model.DateOfExpiration,
                MzrPassportNumber = model.MzrPassportNumber,
                PassportNumberChecksum = model.PassportNumberChecksum,
                MzrNationalityCode = model.MzrNationalityCode,
                MzrDateOfBirth = model.MzrDateOfBirth,
                DOBChecksum = model.DOBChecksum,
                Sex = model.Sex,
                PassportExpiration = model.PassportExpiration,
                ExpirationChecksum = model.ExpirationChecksum,
                PersonalNumber = model.PersonalNumber,
                PersonalNumberChecksum = model.PersonalNumberChecksum,
                FinalChecksum = model.FinalChecksum,
                PassportNumberChecksumValid = model.PassportNumberChecksumValid,
                DateOfBirthChecksumValid = model.DateOfBirthChecksumValid,
                PassportExpirationChecksumValid = model.PassportExpirationChecksumValid,
                PersonalNumberChecksumValid = model.PersonalNumberChecksumValid,
                FinalChecksumValid = model.FinalChecksumValid,
                GenderCrossCheckValid = model.GenderCrossCheckValid,
                DateOfBirthCrossCheckValid = model.DateOfBirthCrossCheckValid,
                PassportExpCrossCheckValid = model.PassportExpCrossCheckValid,
                NationalityCrossCheckValid = model.NationalityCrossCheckValid,
                PassportNoCrossCheckValid = model.PassportNoCrossCheckValid,
            };
            _validatePassport.Validate(passport);

            model.PassportNumberChecksumValid = passport.PassportNumberChecksumValid;
            model.DateOfBirthChecksumValid = passport.DateOfBirthChecksumValid;
            model.PassportExpirationChecksumValid = passport.PassportExpirationChecksumValid;
            model.PersonalNumberChecksumValid = passport.PersonalNumberChecksumValid;
            model.FinalChecksumValid = passport.FinalChecksumValid;
            model.GenderCrossCheckValid = passport.GenderCrossCheckValid;
            model.DateOfBirthCrossCheckValid = passport.DateOfBirthCrossCheckValid;
            model.PassportExpCrossCheckValid = passport.PassportExpCrossCheckValid;
            model.NationalityCrossCheckValid = passport.NationalityCrossCheckValid;
            model.PassportNoCrossCheckValid = passport.PassportNoCrossCheckValid;
        }



    }
}
