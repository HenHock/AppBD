SELECT * FROM Outcomes

CREATE VIEW ClassesShipView AS
SELECT Ship.name AS 'Имя', Classes.Name_cl AS 'Класс', Ship.launched AS 'Год спуска'
FROM Ship INNER JOIN Classes
ON Ship.cod_cl = Classes.Cod_cl

CREATE VIEW ShipClassesView AS
SELECT Ship.name AS 'Имя', Classes.Name_cl AS 'Класс', 
Classes.Type_cl AS 'Тип класса', Classes.Country AS 'Страна', Classes.Numguns AS 'Количество орудий', 
Classes.bore AS 'Калибр', Classes.displacement AS 'Водоизмещение',Ship.launched AS 'Год спуска'
FROM Ship INNER JOIN Classes
ON Ship.cod_cl = Classes.Cod_cl

CREATE VIEW BattleOutcomeView AS
SELECT Outcomes.Cod_ships, Battles.name AS 'Битва', datas AS 'Дата',Outcomes.Result AS 'Результат'
FROM Battles INNER JOIN Outcomes
ON Battles.cod_battles = Outcomes.cod_batles

CREATE VIEW BattleShipView AS
SELECT Ship.name AS 'Имя корабля', Битва, Результат
FROM Ship INNER JOIN BattleOutcomeView
ON Ship.Cod_ships = BattleOutcomeView.Cod_ships

DROP VIEW ClassesShipView


SELECT * FROM BattleShipView