public class ExtPlayer : Player
{
	private Action innerChanged;

	// - Убран вызов делегата в момент подписки, так как это нарушает саму идею делегата Changed.
	// - Потенциальная проблема с отсутствием передачи данных о том, что изменилось. Подписчикам придется обращаться
	//	 непосредственно к объекту для получения доп.информации, что в свою очередь, увеличивает связность
	// - Наименование поля слишком общее, желательно детализировать названием компонента, который изменился
	public event Action Changed {
		add {
			innerChanged += value;
		}
		remove {
			innerChanged -= value;
		}
	}
}

// Не нравится, статическая природа полей и методов класса
class ExtProgram : Program
{
	// Виджет, отображающий игроку здоровье.
	private static TextView healthView = new TextView();

	private static int? previousHealth;

	public static void ExtMain(string[] args)
	{
		// Вызов кода по созданию игрока.
		Main(args);

		player.Changed += OnPlayerChanged;

		// Ударяем игрока.
		HitPlayer();
	}

	// Аналогичные вопросы как и к Example2_1
	private static void OnPlayerChanged()
	{
		healthView.Text = player.Health.ToString();

		// Видно, что View не покраснеет при первом изменении здоровья
		if (previousHealth != null && player.Health - previousHealth < -10) {
			healthView.Color = Color.Red;
		} else {
			healthView.Color = Color.White;
		}
		previousHealth = player.Health;
	}
}
