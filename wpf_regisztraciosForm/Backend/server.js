const express = require('express');
const mysql = require('mysql2');
const app = express();

const port = 3000;
const host = 'localhost'

//megadjuk, hogy hol fusson a szerverünk
app.listen(port,host,()=>{
    console.log(`Listening at http://${host}:${port}`)
});

//egyszerű adatkérés a szervertől
app.get('/',(req,res)=>{
    res.send('A szerver fut!')
});

app.use(express.json());
app.use(express.urlencoded({extended:true}));

//beállítjuk az adatbázist
const connection = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: '',
    database: 'regisztracio'
})

//adatbázis kapcsolat ellenőrzése
app.get('/test',(req,res)=>{
    connection.query('SELECT 1', (err,result)=>{
        if (err){
            console.error('Adatbázis hiba',err)
            return res.status(500).send('Nem sikerült a kapcsolódás az adatbázissal!')
        }
        res.send('A kapcsolat az adatbázissal létrejött!')
    })
});

//összes adat lekérése
app.get('/all',(req,res)=>{
    connection.query('SELECT * FROM felhasznalok', (err,result)=>{
        if (err){
            console.error('Adatbázis hiba',err)
            return res.status(500).send('Hiba a lekérdezés során!')
        }
        if (result.length===null) {
            return res.status(404).json({error: 'Nincs adat!'});
        }
        return res.status(200).json(result);
    })
});

//új felhasználó felvétele
app.post('/users',(req,res)=>{
    const {nev, email, szul_datum, jelszo} = req.body;
    if (!nev || !email || !szul_datum || !jelszo) {
        return res.status(400).json({error: 'Minden mezőt tölts ki!'})
    }
    connection.query('INSERT INTO felhasznalok (id, nev, email, szul_datum, jelszo) VALUES (NULL,?,?,?,?)',
    [nev,email,szul_datum,jelszo],(err,result)=>{
        if (err){
            return res.status(500).send('Adatbázis hiba!');
        }
        
        return res.status(201).json({
            message: 'Sikeres feltöltés!',
            id: result.insertId
        })
    })
});

//felhasználó módosítása
app.put('/users/:id',(req,res)=>{
    const id = Number(req.params.id);
    const {nev, email, szul_datum, jelszo} = req.body;

    if(isNaN(id) || id<=0){
        return res.status(400).json({error:"Hibás ID!"})
    }

    if (!nev || !email || !szul_datum || !jelszo) {
        return res.status(400).json({error: 'Minden mezőt tölts ki!'})
    }
    const sql = `UPDATE felhasznalok SET nev = ?, email = ?, szul_datum=?, jelszo=? WHERE id=?`
    connection.query(sql, [nev,email,szul_datum,jelszo,id],(err,result)=>{
        if (err){
            return res.status(500).send('Adatbázis hiba!');
        }
        if (result.affectedRows === 0) {
            return res.status(404).json({error:"Nincs ilyen ID-jú felhasználó!"});
        }

        return res.status(200).json({message: 'Sikeres módosítás!'});
    })
});

//felhasználó törlése
app.delete('/users/:id',(req,res)=>{
    const id = Number(req.params.id);

    if(isNaN(id) || id<=0){
        return res.status(400).json({error:"Hibás ID!"})
    }
    
    const sql = `DELETE FROM felhasznalok WHERE id=?`;
    connection.query(sql, [id],(err,result)=>{
        if (err){
            return res.status(500).send('Adatbázis hiba!');
        }
        if (result.affectedRows === 0) {
            return res.status(404).json({error:"Nincs ilyen ID-jú felhasználó!"});
        }

        return res.status(200).json({message: 'Sikeres törlés!'});
    })
});


