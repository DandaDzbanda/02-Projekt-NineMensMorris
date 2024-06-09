# 02-Projekt-NineMensMorris
#Dokumentace pro hru Mlýny
Tento kód je převedení deskové hry Mlýny do prostředí WPF. Hlavní třída je MainWindow, která řídí celou hru a uživatelské rozhraní. Když se hra spustí, vytvoří se nová instance pomocí třídy Game. V MainWindow jsou metody pro aktualizaci UI a zpracování kliknutí na hrací desce.

Když hráč klikne na nějakou pozici na desce, metoda Position_Click zkontroluje, jestli tah je platný a podle toho aktualizuje UI. Hra má dvě hlavní fáze: umisťování kamenů a pohyb kamenů. Pokud je hráč ve fázi umisťování kamenů, může umístit nový kámen na volné místo. Pokud je ve fázi pohybu, může přesunout svůj kámen na sousední volné místo.

Metoda UpdateUI projde všechny pozice na desce a nastaví jejich barvu podle toho, který hráč je obsadil. Pokud je pozice volná, nastaví se průhlednost. Taky se aktualizuje textová informace o aktuální fázi hry a aktuálním hráči.

Když hráč klikne na svůj kámen, metoda SelectPiece zkontroluje, jestli je tah možný, a pokud ano, označí ho jako vybraný. Metoda MovePiece pak přesune kámen na novou pozici, pokud je tah platný. Při každém tahu se kontroluje, jestli hráč vytvořil mlýn (řadu tří kamenů). Pokud ano, hráč může odstranit soupeřův kámen.

Ve hře je taky přidána logika pro určení vítěze. Pokud některý hráč má méně než tři kameny nebo nemůže provést žádný tah, hra končí a fáze se změní na GameOver, a CurrentPlayer bude ten co vyhrál.

Pro lepší přehlednost jsem taky přidal barvy pro texty: text "Player 1" je červený a text "Player 2" je modrý. To pomáhá hráčům lépe se orientovat, kdo je na tahu.

Celkově tahle hra poskytuje pocit jako při hraní hry Mlýny. 


