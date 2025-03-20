using BlazorSpinner;
using BootstrapBlazor.Components;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CitizenManagment;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.SurveyVote;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.CitizenManagement;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.Claims;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.SurveyVoteQA.QA
{
    public class QABase : ComponentBase
    {
        public bool isUserInternal { get; set; } = true;
        public string associationId { get; set; }
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        private IJSObjectReference? module;
        public RegisterVoteSurveyListDto registerVoteSurveyListDto = new RegisterVoteSurveyListDto();
        public List<TempAnswerTextOther> tempAnswerTextOthers = new List<TempAnswerTextOther>();
        //public List<RegisterVoteSurvey> ListRegisterVote = new List<RegisterVoteSurvey>();
        public bool isHiddenBtn = true;
        public bool isHiddenDiv = false;

        [Inject]
        public CustomAuthService _customAuthService { get; set; }

        public bool disabledSend = true;


        [NotNull]
        public ImagePreviewer? ImagePreviewer { get; set; }
        public List<string> PreviewList { get; set; } = ["https://upload.wikimedia.org/wikipedia/commons/d/d3/Soccerball.svg"];

        [Parameter]
        public string Id { get; set; }
        public SurveyResponse ModelDefinition { get; set; } = new SurveyResponse();

        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]

        public ISurveyService _surveyService { get; set; }

        public double _ProgressValue = 0;
        public int totalQ = 0;
        public int clickItems = 0;

        public string url = "https://upload.wikimedia.org/wikipedia/commons/d/d3/Soccerball.svg";



        public DotNetObjectReference<QABase> self;
        public void Add(int interval)
        {
            clickItems += interval;
            clickItems = Math.Min(totalQ, Math.Max(0, clickItems));

            var sum = (clickItems * 100) / totalQ;

            _ProgressValue = (double)sum;

            if (clickItems == totalQ)
            {
                disabledSend = false;
            }
            else
            {
                disabledSend = true;
            }

            StateHasChanged();
        }

        public void Add(FocusEventArgs e, string justification)
        {
            if (!string.IsNullOrEmpty(justification))
            {
                Add(1);
            }
        }


        protected async override Task OnInitializedAsync()
        {
            var userTokenData = await _customAuthService.GetClaims();
            if (userTokenData != null)
            {
                var userIdUnique = userTokenData.Claims.FirstOrDefault(c => c.Type == "UserId");
                var ROLE = userTokenData.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
                if (userIdUnique != null)
                {
                    if (!string.IsNullOrEmpty(userIdUnique.Value))
                    {
                        registerVoteSurveyListDto.UserId = Guid.Parse(userIdUnique.Value);
                    }
                }

                if (ROLE != null && ROLE.Value == ROLEAUDITORIA.Usuario)
                {
                    isUserInternal = false;
                }

            }

            await SearchData();
        }


        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import",
              "./Pages/SurveyVoteQA/QA/QA.razor.js");
                self = DotNetObjectReference.Create(this);

                await module.InvokeVoidAsync("GetIp", self);
            }

        }
        private async Task SearchData()
        {
            var responseListManagement = await _surveyService.GetSurveyForVoteById(Id);
            if (responseListManagement != null && responseListManagement.response != null && responseListManagement.response.Success)
            {
                // carga la data
                ModelDefinition = responseListManagement.definition;
                if (ModelDefinition != null && ModelDefinition.SurveyQuestions.Count > 0)
                {
                    totalQ = ModelDefinition.SurveyQuestions.Count;
                }
                StateHasChanged();
            }
            else
            {
                //error
                var message = responseListManagement != null && responseListManagement.response != null ?
                    responseListManagement.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Error", message, autoHide: true);
            }
        }


        public Task View(string url)
        {
            if (PreviewList.Count == 0)
            {
                PreviewList.Add(url);
            }
            else
            {
                PreviewList.Clear();

                PreviewList.Add(url);

            }
            StateHasChanged();
            ImagePreviewer.Show();

            return Task.CompletedTask;
        }


        //public Task PickAnswer(SurveyQuestionOptionResponse answer, string questionId)
        //{
        //    //para las simples se realiza el voto y se le agrega un elemento a la lista, no más de uno
        //    if (ListRegisterVote.Count == 0)
        //    {
        //        ListRegisterVote.Add(new RegisterVoteSurvey
        //        {
        //            SurveyQuestionOptionId = new List<Guid>()
        //            {
        //                answer.Id
        //            },
        //            SurveyQuestionId = Guid.Parse(questionId)
        //        });
        //        Add(1);
        //    }
        //    else
        //    {
        //        var idQ = Guid.Parse(questionId);
        //        var item = ListRegisterVote.FirstOrDefault(x => x.SurveyQuestionId == idQ);
        //        if (item != null)
        //        {
        //            item.SurveyQuestionOptionId.Clear();
        //            item.SurveyQuestionOptionId = new List<Guid>()
        //            {
        //                answer.Id
        //            };
        //        }
        //        else
        //        {
        //            ListRegisterVote.Add(new RegisterVoteSurvey
        //            {
        //                SurveyQuestionOptionId = new List<Guid>()
        //                {
        //                    answer.Id
        //                },
        //                SurveyQuestionId = Guid.Parse(questionId)
        //            });
        //            Add(1);
        //        }

        //    }



        //    if (tempAnswerTextOthers.Count > 0)
        //    {
        //        var exist = tempAnswerTextOthers.FirstOrDefault(x => x.IdAnswer == answer.Id);
        //        if (exist != null)
        //        {
        //            var list = ListRegisterVote.FirstOrDefault(x => x.SurveyQuestionOptionId.First() == answer.Id);
        //            list.CommentValue = exist.TextAnswer;
        //        }
        //    }


        //    return Task.CompletedTask;

        //}
        [JSInvokable]
        public void returnValue(string value, string idAnswer)
        {
            //var id = Guid.Parse(idAnswer);
            //if (ListRegisterVote.Count > 0)
            //{

            //    var existAnswer = ListRegisterVote.FirstOrDefault(x => x.SurveyQuestionOptionId.First() == id);
            //    if (existAnswer != null)
            //    {
            //        if (!string.IsNullOrEmpty(value))
            //        {
            //            existAnswer.CommentValue = value.Trim();
            //        }
            //    }
            //}
            //else
            //{
            //    if (tempAnswerTextOthers.Count > 0)
            //    {

            //        var exits = tempAnswerTextOthers.FirstOrDefault(x => x.IdAnswer == id);
            //        if (exits != null)
            //        {
            //            if (!string.IsNullOrEmpty(value))
            //            {
            //                exits.TextAnswer = value.Trim();
            //            }
            //        }
            //        else
            //        {
            //            if (!string.IsNullOrEmpty(value))
            //            {
            //                tempAnswerTextOthers.Add(new TempAnswerTextOther
            //                {
            //                    IdAnswer = id,
            //                    TextAnswer = value.Trim()
            //                });
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (!string.IsNullOrEmpty(value))
            //        {
            //            tempAnswerTextOthers.Add(new TempAnswerTextOther
            //            {
            //                IdAnswer = id,
            //                TextAnswer = value.Trim()
            //            });
            //        }
            //    }
            //}

            //StateHasChanged();
        }



        public async void HandleFocusOut(FocusEventArgs e, EditContext formContext)
        {
            bool formIsValid = formContext.Validate();
            if (formIsValid == false)
            {
                disabledSend = true;
                return;
            }

            if (clickItems == totalQ)
            {
                disabledSend = false;
            }
        }


        public Task Animacion()
        {
            if (clickItems == totalQ)
            {
                Task.Delay(20000);
                isHiddenDiv = true;

            }
            return Task.CompletedTask;
        }


        public void OnRadiochange(object sender, SurveyQuestionResponse surveyQuestionResponse)
        {

        }

        /// para el radio
        public void SelectionChanged(EventArgs e, SurveyQuestionResponse surveyQuestionResponse)
        {
            var selection = (ChangeEventArgs)e;
            var itemSelected = (string)selection.Value;
            var guid = Guid.Parse(itemSelected);
            //para asegurar que solo si está vacio agrega


            if (string.IsNullOrEmpty(surveyQuestionResponse.ItemUniqueSelected))
            {
                Add(1);
            }

            surveyQuestionResponse.ItemUniqueSelected = (string)itemSelected;
            foreach (var item in surveyQuestionResponse.SurveyQuestionOptions)
            {
                if (item.Id == guid)
                {
                    item.Checked = true;
                }
                else
                {
                    item.Checked = false;
                }
            }



            StateHasChanged();
        }

        /// para el checkbox
        public void SelectionChangedCheckBox(EventArgs e, SurveyQuestionResponse surveyQuestionResponse, SurveyQuestionOptionResponse state)
        {
            var selection = (ChangeEventArgs)e;


            if ((bool)selection.Value)
            {
                var listItems = surveyQuestionResponse.SurveyQuestionOptions.Where(x => x.Checked == true).ToList();
                if (listItems != null && listItems.Count() == 0)
                {
                    Add(1);
                    StateHasChanged();
                }
            }
            else
            {
                var listItems = surveyQuestionResponse.SurveyQuestionOptions.Where(x => x.Checked == true).ToList();
                if (listItems != null)
                {
                    //si es mayor a uno no me importa porque no ocupo ir a quitarlo, el caso de quitar es cuando: solo quede uno y este sea el mismo del evento ( EventArgs e)
                    if (listItems.Count == 1)
                    {
                        var isActualItemUnchecked = listItems.FirstOrDefault(x => x.Id == state.Id);
                        if (isActualItemUnchecked != null)
                        {
                            Add(-1);
                            StateHasChanged();
                        }
                    }

                }
            }

        }

        //public void changeValueInput(SurveyQuestionResponse surveyQuestionResponse)
        //{
        //    surveyQuestionResponse.ItemUniqueSelected = surveyQuestionOptionResponse.Id.ToString();
        //    StateHasChanged();
        //}
        //public async void HandleSend()
        //{
        //    bool banderaNull = false;
        //    registerVoteSurveyListDto.ListRegisterVote = ListRegisterVote;
        //    registerVoteSurveyListDto.SurveyId = ModelDefinition.Id;

        //    /* foreach(var Qa in ListRegisterVote)
        //     {
        //         var exist = ModelDefinition.SurveyQuestions.FirstOrDefault(x => x.Id == Qa.SurveyQuestionId);
        //         if(exist != null)
        //         {
        //             var answer=exist.SurveyQuestionOptions.FirstOrDefault(x=>Qa.SurveyQuestionOptionId == Qa.SurveyQuestionOptionId);
        //             if(answer != null)
        //             {
        //                 if (answer.IsOtherValue)
        //                 {
        //                     if (string.IsNullOrEmpty(Qa.CommentValue))
        //                     {
        //                        banderaNull = true;
        //                     }
        //                 }
        //             }
        //         }
        //     }
        //     */
        //    banderaNull = ListRegisterVote.Any(Qa =>
        //    {
        //        var exist = ModelDefinition.SurveyQuestions.FirstOrDefault(x => x.Id == Qa.SurveyQuestionId);
        //        return exist?.SurveyQuestionOptions.Any(answer =>
        //            answer.Id == Qa.SurveyQuestionOptionId.First() && answer.IsOtherValue && string.IsNullOrEmpty(Qa.CommentValue)) == true;
        //    });


        //    if (!banderaNull)
        //    {
        //        var resul = await _surveyService.SendSurveyOrVote(registerVoteSurveyListDto);

        //        var userIsNull = registerVoteSurveyListDto.UserId.HasValue;
        //        if (resul != null && resul.response.Success)
        //        {
        //            await _toastService.Success("", resul.response.Message, true);
        //            if (userIsNull)
        //            {
        //                if (isUserInternal)
        //                {
        //                    _navigation.NavigateTo("/participacionMunicipio");
        //                }
        //                else
        //                {
        //                    _navigation.NavigateTo("/participacionCiudadana");
        //                }
        //            }
        //            else
        //            {
        //                _navigation.NavigateTo("/");
        //            }

        //        }
        //    }
        //    else
        //    {
        //        await _toastService.Warning("Por favor", "todas la opciones marcadas como otro, indicar un comentario", autoHide: true);
        //    }

        //}



        public async void HandleSend(EditContext formContext)
        {
            bool formIsValid = formContext.Validate();
            if (formIsValid == false)
                return;
            _spinnerService.Show();

            List<RegisterVoteSurvey> registerVoteSurveys = new List<RegisterVoteSurvey>();

            foreach (var surveyQustion in ModelDefinition.SurveyQuestions)
            {
                //si es de pregunta selccion unica
                if (surveyQustion.Type == SURVEYQUESTIONTYPE.SINGLE)
                {
                    //si es el valor para escribir
                    var findOtherValueSelected = surveyQustion.SurveyQuestionOptions.FirstOrDefault(x => x.IsOtherValue);
                    //si el itemuniqueselected es igual al del tipo de otro vamos a preguntar si agrego un comentario
                    if (findOtherValueSelected != null && surveyQustion.ItemUniqueSelected == findOtherValueSelected.Id.ToString())
                    {
                        if (findOtherValueSelected.CommentValue == null || string.IsNullOrEmpty(findOtherValueSelected.CommentValue.Trim()))
                        {
                            await _toastService.Warning("Acción", "Si seleccionaste una opción de respuesta adicional, por favor indicar el comentario ", autoHide: true);
                            _spinnerService.Hide();
                            return;
                        }

                        //si no hay ninguno que sea de otro o no es el unique agregamos normal
                        List<Guid> guidsSelected = new List<Guid>();
                        var item = surveyQustion.SurveyQuestionOptions.FirstOrDefault(x => x.Checked);
                        if (item != null)
                        {
                            guidsSelected.Add(item.Id);
                            registerVoteSurveys.Add(new RegisterVoteSurvey()
                            {
                                SurveyQuestionId = surveyQustion.Id.Value,
                                SurveyQuestionOptionId = guidsSelected,
                                CommentValue = findOtherValueSelected.CommentValue,
                                JustificationValue = surveyQustion.Justification,
                            });
                        }
                    }
                    else
                    {
                        //si no hay ninguno que sea de otro o no es el unique agregamos normal
                        List<Guid> guidsSelected = new List<Guid>();
                        var item = surveyQustion.SurveyQuestionOptions.FirstOrDefault(x => x.Checked);
                        if (item != null)
                        {
                            guidsSelected.Add(item.Id);
                            registerVoteSurveys.Add(new RegisterVoteSurvey()
                            {
                                SurveyQuestionId = surveyQustion.Id.Value,
                                SurveyQuestionOptionId = guidsSelected,
                                JustificationValue = surveyQustion.Justification,
                            });
                        }
                    }
                }
                else
                {
                    //si es el valor para escribir
                    var findOtherValueSelected = surveyQustion.SurveyQuestionOptions.FirstOrDefault(x => x.IsOtherValue);
                    //buscar solo los marcados
                    var itemListSelecteds = surveyQustion.SurveyQuestionOptions.Where(x => x.Checked).ToList();
                    //validar que existan items seleccionados, solo por cualquier caso
                    if (itemListSelecteds != null)
                    {
                        if (findOtherValueSelected != null)
                        {
                            //si encuentra el item seleccionado validamos
                            var isOtherValueInListSelected = itemListSelecteds.FirstOrDefault(x => x.Id == findOtherValueSelected.Id);
                            if (isOtherValueInListSelected != null)
                            {
                                if (findOtherValueSelected.CommentValue == null || string.IsNullOrEmpty(findOtherValueSelected.CommentValue.Trim()))
                                {
                                    await _toastService.Warning("Acción", "Si seleccionaste una opción de respuesta adicional, por favor indicar el comentario ", autoHide: true);
                                    _spinnerService.Hide();
                                    return;
                                }

                                //si todo bien agregamos los ids 
                                var listGuidsSelecteds = itemListSelecteds.Select(x => x.Id).ToList();

                                registerVoteSurveys.Add(new RegisterVoteSurvey()
                                {
                                    SurveyQuestionId = surveyQustion.Id.Value,
                                    SurveyQuestionOptionId = listGuidsSelecteds,
                                    CommentValue = findOtherValueSelected.CommentValue,
                                    JustificationValue = surveyQustion.Justification,
                                });
                            }
                            else
                            {
                                //si no hay item de comentario lo dejamos así y agremos los ids 
                                var listGuidsSelecteds = itemListSelecteds.Select(x => x.Id).ToList();

                                registerVoteSurveys.Add(new RegisterVoteSurvey()
                                {
                                    SurveyQuestionId = surveyQustion.Id.Value,
                                    SurveyQuestionOptionId = listGuidsSelecteds,
                                    JustificationValue = surveyQustion.Justification,
                                });
                            }
                        }
                        else
                        {
                            //si no hay item de comentario lo dejamos así y agremos los ids 
                            var listGuidsSelecteds = itemListSelecteds.Select(x => x.Id).ToList();

                            registerVoteSurveys.Add(new RegisterVoteSurvey()
                            {
                                SurveyQuestionId = surveyQustion.Id.Value,
                                SurveyQuestionOptionId = listGuidsSelecteds,
                                JustificationValue = surveyQustion.Justification,
                            });
                        }
                    }
                    else
                    {
                        await _toastService.Warning("Acción", "Selecciona de la opción múltiple alguna opción", autoHide: true);
                        _spinnerService.Hide();
                        return;
                    }
                }
            }

            registerVoteSurveyListDto.ListRegisterVote = registerVoteSurveys;
            registerVoteSurveyListDto.SurveyId = ModelDefinition.Id;

            var resul = await _surveyService.SendSurveyOrVote(registerVoteSurveyListDto);
            var userIsNull = registerVoteSurveyListDto.UserId.HasValue;
            if (resul != null && resul.response.Success)
            {
                _spinnerService.Hide();
                await _toastService.Success("Acción", resul.response.Message, true);
                if (userIsNull)
                {
                    if (isUserInternal)
                    {
                        _navigation.NavigateTo("/participacionMunicipio");
                    }
                    else
                    {
                        _navigation.NavigateTo("/participacionCiudadana");
                    }
                }
                else
                {
                    _navigation.NavigateTo("/");
                }
            }
            else
            {
                _spinnerService.Hide();
                var message = resul != null && resul.response != null ? resul.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }



        }

        [JSInvokable]
        public void GetIpReturn(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                registerVoteSurveyListDto.Ip = value;

            }
        }

        public async Task goToList()
        {
            var userIsNull = registerVoteSurveyListDto.UserId.HasValue;
            if (userIsNull)
            {
                _navigation.NavigateTo("javascript:history.back()");
            }
            else
            {
                _navigation.NavigateTo("/");
            }
        }
        public string GetTypeToShow(string item)
        {
            if (item == SURVEYQUESTIONTYPE.SINGLE)
            {
                return "(Selección Única)";
            }
            else
            {
                return "(Selección Múltiple)";
            }
        }

    }

}

