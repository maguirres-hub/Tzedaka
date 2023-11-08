using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Tzedaka.Model;

namespace Tzedaka.ViewModels
{
    public class ViewModel_Curso
    {
        HttpClient clienteHttp;
        private ObservableCollection<Cursos> Cursos_ = new ObservableCollection<Cursos>();
        private Cursos Curso_ = new Cursos();

        private bool IsLoading_;




        public ViewModel_Curso()
        {
            Cargar_Cursos();
        }

        private async void Cargar_Cursos()
        {
            await Get_Cursos();
        }
        public async Task Get_Cursos()
        {
            IsLoading = true;
            try
            {
                clienteHttp = new HttpClient();
                clienteHttp.Timeout = TimeSpan.FromSeconds(10);

                var url = "https://berajotweb.com/tzedakin/api/cursos/";

                HttpResponseMessage respuesta = await clienteHttp.GetAsync(url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var CursosString = await respuesta.Content.ReadAsStringAsync();
                    var CursosDes = JsonConvert.DeserializeObject<ObservableCollection<Cursos>>(CursosString);
                    if (CursosDes.Count > 0)
                    {
                        Cursos = CursosDes;
                        Debug.WriteLine(Cursos[0].URl_Video.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            IsLoading = false;
        }


        public bool IsLoading
        {
            get { return IsLoading_; }
            set
            {
                if (IsLoading_ != value)
                {
                    IsLoading_ = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        public ObservableCollection<Cursos> Cursos
        {
            get { return Cursos_; }
            set
            {
                if (Cursos_ != value)
                {
                    Cursos_ = value;
                    OnPropertyChanged(nameof(Cursos));
                }
            }
        }

        public Cursos Curso
        {
            get { return Curso_; }
            set
            {
                if (Curso_ != value)
                {
                    Curso_ = value;
                    OnPropertyChanged(nameof(Curso));

                }
            }
        }






        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
