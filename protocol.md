# Protocol for MTCG

## Design

I structured my solution similarly to the Http-Server we designed in class. There are multiple projects, namely:

- **MonsterTradingCards.Server.Core**
  - This project specifically handles everything regarding the actual communication.
- **MonsterTradingCards.Server**
  - This project, which also contains the actual server (and therefore also the Program.cs to compile to a executable program). It uses the _MonsterTradingCards.Server.Core_-project to build the HttpServer, and defines the routes as well as the backend logic for them. In here, there also is the Game logic, as it technically is part of the backend-logic for the battle-command.
- **MonsterTradingCards.Server.Test**
  - This project is specifically made to contain all of the unit test for the other projects.

In the Core-project - which handles the Http-Server - I basically only changed the code to handle clients in their own threads, so multiple users can be handled at the same time. Other than that, I basically left it as we made it in class.

The Server-project implements the Command-Pattern as well as the Repository-Pattern. The latter pattern means that Database-access is "abstracted" for the commands, which only interact with the DB through managers. The Command-pattern on the other hand means, that for every endpoint of our server, a dedicated command class (which can be public or restricted) is made, which is then registered into a router which "connects" the client request to the right command.

The part of the code I am most proud of is also part of a problem that I have mentioned in the next section. In the BattleHandler, after fetching the players' decks, the handler still needs to convert the cards as saved in the database to cards that can be used by the BattleLogic. Therefore, I wrote some code to convert the strings saved in the DB into the classes needed for the GameCard class (ElementType and CardType).

```c#
    public static IElementType convertStringToElementType(string elementTypeString)
    {
        if (elementTypeString == "Regular")
        {
            elementTypeString = "Normal";
        }

        var elementType = typeof(IElementType).Assembly.GetTypes().Where(t => t.Name == elementTypeString + "ElementType").FirstOrDefault();
        return (IElementType)Activator.CreateInstance(elementType);
    }
```

```c#
    public static ICardType convertStringToCardType(string cardTypeString)
    {
        if (cardTypeString == "Spell")
        {
            return new SpellType();
        }

        var elementType = typeof(ICardType).Assembly.GetTypes().Where(t => t.Name == cardTypeString + "MonsterType").FirstOrDefault();
        return (ICardType)Activator.CreateInstance(elementType);
    }
```

These two methods are part of a public utility class, which take a string and try to convert it into the right classes. In order to achieve this, I used the Assembly/Reflection functionality of C# in order to find all of the interfaces that implement the interfaces (`ICardType` and `IElementType`) and then try to find the class where the name fits the string. As will be mentioned below this problem arised due to me not planning ahead. When I arrived at this point, I had to either create this code for conversion or change the GameCard code to do the specialty and element consideration some other way. As visible above I decided for the first option, which did cost me some time, however it also was a challenge for me and resulted in this part of the code, which I am most proud of (although it is a fix to cover up my previous mistake of not planning ahead :) )

## Lessons Learned

Throughout the project it became obvious to me, that I probably should have spent more time developing a design for the Database. While I did think about some parts of the Database-design, I noticed during development that often times, I did not yet know exactly how I wanted to handle certain things (e.g. persisting packages, saving of stats). This didn't really cause any issues, it just cost time which could have been saved if I had thought about it beforehand.

Through my presentation presenting progress during one of the lessons, I learned that in the Command-Pattern, if possible one should not write any logic-code in the commands themselves, which should basically only check conditions and call manager methods. Following the presentation, I reworked some of the code - especially the Battle lobby handling, which I created a class of its own for - in order to follow that principle.

Since I wrote my Game logic before I implemented the commands, I also implemented the logic for considering card specialties and element types, however I did so in a very complicated way. At first, this didn't seem to be a problem, however I noticed some difficulties when it came to convert a card from the card string to have the correct card and element type. I did find a solution in the end, however it did cost me some time looking things up and trying stuff out. This time could at least have partially been saved had I thought about this thoroughly first.

## Unit Test Strategy

I decided to make unit tests for the GameLogic, GameHandler, GameCard, as well as the different managers's methods that do more than just calling a repository function (e.g. create a new object, check requirements, etc). I did not test the Repositories, as I unfortunately did not have time to either create a dedicated InMemoryDatabase, or Mock the Database connection. I did decide not to test with the Postgresql database, because I was concerned with data persistence, as a unit-test could likely fail because of pre-existing data or similar.

For the unit tests that I wrote, I tried to make sure to use the arrange-act-assert-model. This way, the tests are specifically structured to get an easier overview of the tests, such as which parts are actually tested, and which only prepare for the test.

For the Managers, as well as for the GameHandler, I needed to Mock some repository-classes. While it should be obvious, that the managers need the mocked classes (because the test should only test the manager and should work regardless if the repository works), the GameHandler needs the Deck and Card-Manager mocked, because the GameHandler fetches the cardIds through the Deck-Manager and then gets the corresponding cards through the Card-Manager.

I think, the parts tested are critical parts of the code, because the whole "game" depends on the GameLogic, GameHandler and GameCard, as well as the manager working as intended. Technically, the repositories also have to work as intended and are therefore crucial code, but as explained before, I unfortunatelly did not have enough time to test these parts extensively.

## Unique Feature

As a unique feature, i chose a so called "sacrifice"-mechanic. It works by sacrificing one card in order to make another stronger. Some requirements need to be met in order to sacrifice a card:

- The card to sacrifice must not be in a deck
- The card to sacrifice must be at least 50% stronger than the receiver card

Are all of these requirements met, a card can be sacrificed and give another card 25% of the sacrificed card's damage. Element or Card type do not matter.

In order to sacrifice a card, one needs to post a request to the server's "sacrifice"-endpoint, with both the sacrifice and receiver card in the request body. The command is also restricted, so authentication is needed. An example of a curl-call can be seen below.

`curl -X POST http://localhost:10001/sacrifice --header "Content-Type: application/json" --header "Authorization: Basic manuel-mtcgToken" -d "{\"Sacrifice\":\"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\", \"Reciever\":\"e85e3976-7c86-4d06-9a80-641c2019a79f\"}"`

## Link to GitHub

[GitHub Repo](https://github.com/broteinheit/MonsterTradingCards)

## Tracked Time

Over the course of this semester, I have spent about 45 hours on this project.

### Working Hours Table

| Date       | Working Hours |
| ---------- | ------------- |
| 01.12.2021 | 2             |
| 30.12.2021 | 2             |
| 03.01.2022 | 2             |
| 04.01.2022 | 2             |
| 05.01.2022 | 4             |
| 07.01.2022 | 2.5           |
| 09.01.2022 | 2.5           |
| 10.01.2022 | 7.5           |
| 14.01.2022 | 2.5           |
| 16.01.2022 | 7             |
| 18.01.2022 | 2             |
| 19.01.2022 | 5             |
| 20.01.2022 | 4             |
