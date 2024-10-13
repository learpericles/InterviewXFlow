using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatElementBehaviour : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Button _button;

    public void Setup(CheatActionDescription description)
    {
        _text.text = description.name;
        _button.onClick.AddListener(() => description.cheatAction());
    }
}

// Менеджер нарушает DIP и SRP принципы, здесь как управление жизненным циклом UI, так и хранение провайдеров читов,
// плюс сам по себе синглтон нарушает SRP.
//
// Желательно,извлечь интерфейс ICheatManager, который бы работал непосредственно с провайдерами.
// Для работы с UI можно создать интерфейс ICheatPanelController, который бы управлял жизненным циклом UI
//
// Также желательно, избавиться от синглтона, чтобы иметь возможность внедрять менеджер как зависимость
//
// Не нравится использование вложенных классов и интерфейсов
//
// Желательно, всю работы с читами держать в отдельной сборке, чтобы иметь возможность не добавлять ее для прод билдов
public class CheatManager
{
    public class CheatActionDescription
    {
        public readonly string name;
        public readonly Action cheatAction;

        public CheatActionDescription(string name, Action cheatAction)
        {
            this.name = name;
            this.cheatAction = cheatAction;
        }
    }

    public interface ICheatProvider
    {
        IEnumerable<CheatActionDescription> GetCheatActions();
    }

    public static readonly CheatManager Instance = new CheatManager();

    private readonly List<ICheatProvider> _providers = new List<ICheatProvider>();

    private GameObject _panelPrefab;
    private CheatElementBehaviour _cheatElementPrefab;

    private GameObject _panel;

    public void Setup(GameObject panelPrefab, CheatElementBehaviour cheatElementPrefab)
    {
        _panelPrefab = panelPrefab;
        _cheatElementPrefab = cheatElementPrefab;
    }

    public void RegProvider(ICheatProvider provider)
    {
        _providers.Add(provider);
    }

    public void ShowCheatPanel()
    {
        if (_panel != null)
            return;

        _panel = UnityEngine.Object.Instantiate(_panelPrefab);
        foreach (var provider in _providers)
        {
            foreach (var cheatAction in provider.GetCheatActions())
            {
                var element = UnityEngine.Object.Instantiate(_cheatElementPrefab, _panel.transform);

                element.Setup(cheatAction);
            }
        }
    }

    public void HideCheatPanel()
    {
        if (_panel == null)
            return;

        UnityEngine.Object.Destroy(_panel);
        _panel = null;
    }
}

// Нарушается SRP принцип. Здесь как предоставление списка читов, так и работа с состоянием поля _health
// Неясный нейминг класса
public class SomeManagerWithCheats : CheatManager.ICheatProvider
{
    private int _health;

	// Прямая зависимость от реализации менеджера
	// Сам по себе менеджер читов уже имеет метод для регистрации провайдера, лучше перенести логику регистрации
	// провайдера в менеджер - это позволит уменьшить связность
	// В рамках примера, метод нигде не вызывается
    public void Setup()
    {
        CheatManager.Instance.RegProvider(this);
    }

	// Методы c yield на уровне IL выглядят как отдельный класс с машиной состояния, чтобы снизить издержки, в рамках
	// данного примера, можно просто возвращать коллекцию читов
    IEnumerable<CheatManager.CheatActionDescription> CheatManager.ICheatProvider.GetCheatActions()
    {
        yield return new CheatManager.CheatActionDescription("Cheat health", () => _health++);
        yield return new CheatManager.CheatActionDescription("Reset health", () => _health = 0);
    }
}
