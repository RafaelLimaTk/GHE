using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GHE.Domain.Entities;
using GHE.Domain.Interfaces;
using GHE.Extensions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.ObjectModel;

namespace GHE.ViewModels.Ghes;

public partial class CreateAndEditGheViewModel : ObservableObject
{
    [ObservableProperty]
    private string searchTerm;

    [ObservableProperty]
    private string matricula;

    [ObservableProperty]
    private string nome;

    [ObservableProperty]
    private string ghe;

    [ObservableProperty]
    private string descricaoAtividades;

    [ObservableProperty]
    private bool insalubridade;

    [ObservableProperty]
    private bool periculosidade;

    [ObservableProperty]
    private bool naoAplica;

    [ObservableProperty]
    private Training newTraining = new Training();

    [ObservableProperty]
    private ObservableCollection<Ghe> gheListFilter;

    private List<Ghe> _gheListFull;

    public ObservableCollection<Training> Trainings { get; } = new ObservableCollection<Training>();

    private readonly ITrainingRepository _trainingRepository;
    private readonly IGheRepository _gheRepository;
    private Ghe _currentGhe;
    public CreateAndEditGheViewModel()
    {
        _trainingRepository = App.Current.Handler.MauiContext.Services.GetService<ITrainingRepository>();
        _gheRepository = App.Current.Handler.MauiContext.Services.GetService<IGheRepository>();

        Sincronization();
    }

    [RelayCommand]
    private void Sincronization()
    {
        _gheListFull = _gheRepository.GetAllIncludeTrainingsAsync().Result.ToList();
        GheListFilter = new ObservableCollection<Ghe>(_gheListFull);
    }

    [RelayCommand]
    private void SearchList()
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
        {
            GheListFilter = new ObservableCollection<Ghe>(_gheListFull);
        }
        else
        {
            var lowerCaseSearchTerm = SearchTerm.ToLower();
            var filteredList = _gheListFull
                .Where(g => g.Matricule.ToLower().Contains(lowerCaseSearchTerm) ||
                            g.Name.ToLower().Contains(lowerCaseSearchTerm) ||
                            g.GHE.ToLower().Contains(lowerCaseSearchTerm))
                .ToList();

            GheListFilter = new ObservableCollection<Ghe>(filteredList);
        }
    }

    [RelayCommand]
    private void AddTraining()
    {
        if (!string.IsNullOrWhiteSpace(NewTraining.TrainingName))
        {
            Trainings.Add(new Training
            {
                TrainingName = NewTraining.TrainingName.Trim(),
                TrainingDate = NewTraining.TrainingDate,
                ASO = NewTraining.ASO,
                TrainingDateFinal = NewTraining.TrainingDateFinal,
            });

            NewTraining = new Training();
            OnPropertyChanged(nameof(NewTraining));
        }
    }

    [RelayCommand]
    private async Task Search()
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Informe a matricula ou nome do GHE", "OK");
            return;
        }

        var ghe = await _gheRepository.GetByMatriculaOrNomeAsync(SearchTerm.Trim());
        if (ghe != null)
        {
            _currentGhe = ghe;
            Matricula = ghe.Matricule;
            Nome = ghe.Name;
            Ghe = ghe.GHE;
            DescricaoAtividades = ghe.Description;
            Insalubridade = ghe.Unhealthiness;
            Periculosidade = ghe.Dangerousness;
            NaoAplica = ghe.NotApplicable;

            Trainings.Clear();
            if (ghe.Id != Guid.Empty)
            {
                foreach (var training in await _trainingRepository.GetByGheIdAsync(ghe.Id))
                {
                    if (training is null)
                        continue;
                    Trainings.Add(training);
                }
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Não existe GHE cadastrado", "OK");
        }
    }

    [RelayCommand]
    private void New()
    {
        Matricula = string.Empty;
        Nome = string.Empty;
        Ghe = string.Empty;
        DescricaoAtividades = string.Empty;
        Insalubridade = false;
        Periculosidade = false;
        NaoAplica = false;
        Trainings.Clear();
        NewTraining = new Training();
        OnPropertyChanged(nameof(NewTraining));
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Matricula) ||
            string.IsNullOrWhiteSpace(Nome) ||
            string.IsNullOrWhiteSpace(Ghe) ||
            string.IsNullOrWhiteSpace(DescricaoAtividades))
        {
            await Application.Current.MainPage.DisplayAlert("Erro", "Preencha todos os campos", "OK");
            return;
        }

        try
        {
            if (_currentGhe != null)
            {
                _currentGhe.Matricule = Matricula.Trim();
                _currentGhe.Name = Nome.Trim();
                _currentGhe.GHE = Ghe.Trim();
                _currentGhe.Description = DescricaoAtividades;
                _currentGhe.Unhealthiness = Insalubridade;
                _currentGhe.Dangerousness = Periculosidade;
                _currentGhe.NotApplicable = NaoAplica;

                await _gheRepository.UpdateAsync(_currentGhe);

                foreach (var training in Trainings)
                {
                    if (training.GheId == Guid.Empty)
                    {
                        training.GheId = _currentGhe.Id;
                        await _trainingRepository.AddAsync(training);
                    }
                    else
                    {
                        await _trainingRepository.UpdateAsync(training);
                    }
                }

                await Application.Current.MainPage.DisplayAlert("Sucesso", "GHE atualizado com sucesso", "OK");
            }
            else
            {
                var newGhe = new Ghe
                {
                    Matricule = Matricula.Trim(),
                    Name = Nome.Trim(),
                    GHE = Ghe.Trim(),
                    Description = DescricaoAtividades,
                    Unhealthiness = Insalubridade,
                    Dangerousness = Periculosidade,
                    NotApplicable = NaoAplica
                };

                await _gheRepository.AddAsync(newGhe);
                _currentGhe = newGhe;

                foreach (var training in Trainings)
                {
                    training.GheId = _currentGhe.Id;
                    await _trainingRepository.AddAsync(training);
                }

                await Application.Current.MainPage.DisplayAlert("Sucesso", "GHE criado com sucesso", "OK");
            }
            New();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Erro", $"Não foi possível salvar o GHE: {ex.Message}", "OK");
        }
    }


    [RelayCommand]
    private async Task Delete()
    {
        if (_currentGhe == null)
        {
            await Application.Current.MainPage.DisplayAlert("Erro", "GHE atual não é válido", "OK");
            return;
        }

        try
        {
            await _gheRepository.DeleteAsync(_currentGhe.Id);
            await Application.Current.MainPage.DisplayAlert("Sucesso", "GHE Deletado com sucesso", "OK");

            New();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Erro", $"Não foi possível deletar o GHE: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task DeleteTraining(Training training)
    {
        if (training == null)
        {
            await Application.Current.MainPage.DisplayAlert("Erro", "Treinamento inválido", "OK");
            return;
        }

        try
        {
            Trainings.Remove(training);

            if (training.GheId != Guid.Empty)
            {
                await _trainingRepository.DeleteAsync(training.Id).ConfigureAwait(false);
            }

            await Application.Current.MainPage.DisplayAlert("Sucesso", "Treinamento excluído com sucesso", "OK");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Erro", $"Não foi possível excluir o treinamento: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    public async Task SaveReport(Guid id)
    {
        try
        {
            var ghe = await _gheRepository.GetByIdAsync(id);
            if (ghe != null)
            {
                string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
                string fileName = $"Relatorio {ghe.Name}";
                if (!string.IsNullOrEmpty(fileName))
                {
                    string filePath = Path.Combine(downloadsPath, fileName + ".pdf");
                    CreatePdfReport(filePath, ghe);
                    await Application.Current.MainPage.DisplayAlert("Success", "PDF criado com sucesso em downloads", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "GHE não encontrado", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Erro ao criar PDF: " + ex.Message, "OK");
        }
    }

    public async Task CreatePdfReport(string filePath, Ghe ghe)
    {
        using (var document = new Document())
        {
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            var title = new Paragraph("Relatório GHE", titleFont)
            {
                Alignment = iTextSharp.text.Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            document.Add(new Paragraph($"Matricula: {ghe.Matricule}"));
            document.Add(new Paragraph($"Nome: {ghe.Name}"));
            document.Add(new Paragraph($"GHE: {ghe.GHE}"));
            document.Add(new Paragraph($"Descrição: {ghe.Description}"));

            if (ghe.Unhealthiness)
                document.Add(new Paragraph($"Insalubridade"));
            if (ghe.Dangerousness)
                document.Add(new Paragraph($"Periculosidade"));
            if (ghe.NotApplicable)
                document.Add(new Paragraph("Não aplicado"));

            document.Add(new Paragraph("Treinamentos:\n\n"));

            var Trainings = await _trainingRepository.GetByGheIdAsync(ghe.Id);

            if (Trainings != null && Trainings.Count() > 0)
            {
                PdfPTable table = new PdfPTable(4) { WidthPercentage = 100 };
                table.AddCell("Treinamento");
                table.AddCell("Data do treinamento");
                table.AddCell("ASO");
                table.AddCell("Data do ASO");

                foreach (var training in Trainings)
                {
                    table.AddCell(training.TrainingName ?? "");
                    table.AddCell(training.TrainingDate.ToString("yyyy-MM-dd") ?? "");
                    table.AddCell(training.ASO ?? "");
                    table.AddCell(training.TrainingDateFinal.ToString("yyyy-MM-dd") ?? "");
                }
                document.Add(table);
            }

            document.Close();
            writer.Close();
        }
    }

    [RelayCommand]
    public async Task SaveAllReports()
    {
        try
        {
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"Relatorio_Todos_GHEs.pdf";
            string filePath = Path.Combine(downloadsPath, fileName);

            var allGheRecords = await _gheRepository.GetAllAsync();
            using (var document = new Document())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                foreach (var ghe in allGheRecords)
                {
                    await CreatePdfReportForAll(ghe, document);
                }

                document.Close();
                writer.Close();
            }

            await Application.Current.MainPage.DisplayAlert("Success", "Todos os PDFs foram criados com sucesso em downloads", "OK");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Erro ao criar PDFs: " + ex.Message, "OK");
        }
    }

    public async Task CreatePdfReportForAll(Ghe ghe, Document document)
    {
        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
        var title = new Paragraph($"Relatório GHE: {ghe.Name}", titleFont)
        {
            Alignment = iTextSharp.text.Element.ALIGN_CENTER,
            SpacingAfter = 20
        };
        document.Add(title);

        if (ghe != null)
        {
            document.Add(new Paragraph($"Matricula: {ghe.Matricule}"));
            document.Add(new Paragraph($"Nome: {ghe.Name}"));
            document.Add(new Paragraph($"GHE: {ghe.GHE}"));
            document.Add(new Paragraph($"Descrição: {ghe.Description}"));

            if (ghe.Unhealthiness)
                document.Add(new Paragraph($"Insalubridade"));
            if (ghe.Dangerousness)
                document.Add(new Paragraph($"Periculosidade"));
            if (ghe.NotApplicable)
                document.Add(new Paragraph("Não aplicado"));
        }

        document.Add(new Paragraph("Treinamentos:\n\n"));

        var trainings = await _trainingRepository.GetByGheIdAsync(ghe.Id);

        if (trainings != null && trainings.Count() > 0)
        {
            PdfPTable table = new PdfPTable(4) { WidthPercentage = 100 };
            table.AddCell("Treinamento");
            table.AddCell("Data do treinamento");
            table.AddCell("ASO");
            table.AddCell("Data do ASO");

            foreach (var training in trainings)
            {
                if (training is not null)
                {
                    table.AddCell(training.TrainingName ?? "");
                    table.AddCell(training.TrainingDate.ToString("yyyy-MM-dd") ?? "");
                    table.AddCell(training.ASO ?? "");
                    table.AddCell(training.TrainingDateFinal.ToString("yyyy-MM-dd") ?? "");
                }
            }
            document.Add(table);
        }

        document.NewPage();
    }

    [RelayCommand]
    public async Task GoToCreateGhe()
    {
        var user = UserAuth.GetUserAuth();
        if (user == null)
        {
            await Shell.Current.GoToAsync("login");
        }
        else
        {
            await Shell.Current.GoToAsync("criarghe");
        }
    }
}
