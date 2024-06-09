using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GHE.Domain.Entities;
using GHE.Domain.Interfaces;
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

    public ObservableCollection<Training> Trainings { get; } = new ObservableCollection<Training>();

    private readonly ITrainingRepository _trainingRepository;
    private readonly IGheRepository _gheRepository;
    private Ghe _currentGhe;
    public CreateAndEditGheViewModel()
    {
        _trainingRepository = App.Current.Handler.MauiContext.Services.GetService<ITrainingRepository>();
        _gheRepository = App.Current.Handler.MauiContext.Services.GetService<IGheRepository>();
    }

    [RelayCommand]
    private void AddTraining()
    {
        if (!string.IsNullOrWhiteSpace(NewTraining.TrainingName) &&
            !string.IsNullOrWhiteSpace(NewTraining.ASO))
        {
            Trainings.Add(new Training
            {
                TrainingName = NewTraining.TrainingName,
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
        var ghe = await _gheRepository.GetByMatriculaOrNomeAsync(SearchTerm);
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
            foreach (var training in await _trainingRepository.GetByGheIdAsync(ghe.Id))
            {
                Trainings.Add(training);
            }
        }
        else
        {
            await Toast.Make("Não existe GHE cadastrado").Show();
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
        if (_currentGhe != null)
        {
            _currentGhe.Matricule = Matricula;
            _currentGhe.Name = Nome;
            _currentGhe.GHE = Ghe;
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
                    training.TrainingDate = training.TrainingDate ?? DateTime.Now;
                    await _trainingRepository.AddAsync(training);
                }
                else
                {
                    await _trainingRepository.UpdateAsync(training);
                }
            }
            await Toast.Make("GHE atualizado com sucesso").Show();
        }
        else
        {
            var newGhe = new Ghe
            {
                Matricule = Matricula,
                Name = Nome,
                GHE = Ghe,
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
            await Toast.Make("GHE criado com sucesso").Show();
        }
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (_currentGhe != null)
        {
            await _gheRepository.DeleteAsync(_currentGhe.Id);
            foreach (var training in Trainings)
            {
                await _trainingRepository.DeleteAsync(training.Id);
            }

            await Toast.Make("GHE Deletado com sucesso").Show();

            New();
        }
        else
        {
            await Toast.Make("Não foi possivel deletar o GHE").Show();
        }
    }

    [RelayCommand]
    private async Task DeleteTraining(Training training)
    {
        if (training != null)
        {
            Trainings.Remove(training);

            if (training.GheId != Guid.Empty)
            {
                await _trainingRepository.DeleteAsync(training.Id);
            }

            await Toast.Make("Treinamento excluído com sucesso").Show();
        }
        else
        {
            await Toast.Make("Não foi possível excluir o treinamento").Show();
        }
    }
}
