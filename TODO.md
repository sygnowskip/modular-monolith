TODO:
1. Persystencja eventów na SaveChanges()
2. Interfejs pod asynchroniczne eventy w mediatorze + robienie nowych scopow na każdy z nich
3. Transakcje na komendach

ISSUES:
1. Co z osobą wykonującą akcję? Co z uprawnieniami w przypadku asynchronicznych eventow?
2. Co z transakcjami? Jeżeli wiele workerów ma działać na tej samej bazie danych, to zachodzie potrzeba lockowania wierszy które się wybrało, żeby obydwa nie procesowały jednocześnie tego samego - jednocześnie, chcemy mieć transakcję per komenda (która może być wywołana w event handlerze)
3. Wymiana EF Core na Dappera w eventach? Luźniejsze zależności