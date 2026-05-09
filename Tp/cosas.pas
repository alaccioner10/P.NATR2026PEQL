//La plataforma de música Spotify desea procesar información de sus artistas. Para ello, dispone de una estructura de datos con información de los artistas. De cada artista se conoce: nombre, 
//género musical (1: Funk, 2: Pop, 3: Rock, 4: Folklore, 5: Cumbia, 6: Cuarteto, 7: Tango, 8: Electrónica), cantidad de reproducciones de todas sus canciones, y si su perfil se encuentra verificado o no.
//Se pide realizar un programa que:
//a. Informe los dos códigos de géneros musicales que poseen más artistas con perfil verificado.
//b. Informe el porcentaje de artistas Pop en los cuales la suma de los dígitos pares de su cantidad de reproducciones es igual que la suma de los dígitos impares.
//c. Genere una nueva estructura que almacene los artistas con perfil verificado con menos de 1 millón de reproducciones;
//Nota: la estructura que se dispone se debe recorrer una vez.

program ejemplo;

type

    generoM=1..8;

    artistas=record
        nombre:string;
        generos:generoM;
        CantRepro:integer;
        verificado:boolean;
    end;

    nodo=record
        dato:artistas;
        sig:^nodo;
    end;

    Lista=^nodo;

    ArtistasVer=record
        nombre:string;
        generos:generoM;
    end;

    nodo2=record
        dato:ArtistasVer;
        sig2:^nodo2;
    end;

    Lista2=^nodo2;
    
    Vgeneros=array [generoM] of integer;

var 
    L:lista;
    L2:Lista2;
    aux:lista;
    v:Vgeneros;
    n,ntotal:integer;



procedure ArmarLista1 (var L:lista); //Se dispone

procedure inicializarVector (var v:Vgeneros);
var
    i:Vgeneros;

begin
    for i:=1 to 8 do
    begin
        v[i]:=0;
    end;
end;

procedure  vcontador (var v:Vgeneros; L:lista);
begin
    if (L^.dato.verificado=True) then
    begin
        v[L^.dato.generos]:=v[L^.dato.generos]+1;
    end;
end;

function Descomponer(numaux:integer):boolean;
var 
    numfun,digito,partotal,impartotal:integer;
begin
    numfun:=numaux;
    partotal:=0;
    impartotal:=0;
    while (numfun > 0) do
    begin
        digito:=numfun mod 10;
        if (digito mod 2 = 0) then
        begin
            partotal:=partotal+digito;
        end
        else
        begin
            impartotal:=impartotal+digito;
        end;
        numfun:=numfun div 10;
    end;
    if (impartotal=partotal) then
    begin
        Descomponer:=true;
    end
    else
    begin
        Descomponer:=false;
    end;
end;

function Calcularporcentaje(n:integer;ntotal:integer):real;
var
    porcentajeaux:real;
begin
    if (ntotal>0) then
    begin
        porcentajeaux:=(n/ntotal)*100;
    end
    else
    begin
        porcentajeaux:=-1;
    end;
    Calcularporcentaje:=porcentajeaux;
end;

procedure buscarporcentaje (l:lista; var n:integer; var ntotal:integer);
var
    numaux:integer;
    condicion:boolean;
    porcentaje:real;
begin
    condicion:=false;
    if (L^.dato.generos=2) then
    begin
        numaux:=L^.dato.CantRepro;
        condicion:=Descomponer(numaux);
        if (condicion=true) then
        begin
            n:=n+1;
        end;
        ntotal:=ntotal+1;
    end;
    porcentaje:=Calcularporcentaje(n,ntotal);
    if (porcentaje<> -1) then
    begin
        writeln(porcentaje, '%');
    end
    else
    begin
        writeln('no hay ninguno');
    end;
end;

procedure comparar (v:Vgeneros;i:genero; var cod1:genero; var cod2:genero; var num1:integer; var num2:integer);
begin
    if (v[i]>num1) then
    begin
        num2:=num1;
        cod2:=cod1;
        num1:=v[i];
        cod1:=i;
    end
    else
    begin
        if (v[i]>num2) then
        begin
            num2:=v[i];
            cod2:=i;
        end;
    end;
end;



procedure leercodigos (v:Vgeneros);
var
    num1,num2:integer;
    i,cod1,cod2:genero;
begin
    num1:=-1;
    num2:=-1;
    for i:=1 to 8 do
    begin
        comparar(v,i,cod1,cod2,num1,num2);
    end;
    writeln(cod1);
    writeln(cod2);
end;

procedure almacenardato (var nuevonodo:lista2; L:lista);
begin
    nuevonodo^.dato.nombre:=L^.dato.nombre;
    nuevonodo^.dato.generos:=L^.dato.generos;
end;

procedure armarlista2 (var L2:Lista2;L:lista);
var
    nuevonodo:lista2;
begin
    new(nuevonodo);
    nuevonodo^.sig:=L2;
    almacenardato(nuevonodo,L);
    L2:=nuevonodo;
end;


procedure Segundalista (var L2:Lista2; L:lista);
begin
    if (L^.dato.CantRepro < 1000000) and (L^.dato.verificado=True) then
    begin
        armarlista2(L2,L);
    end;
end;

begin
    aux:=L;
    L2:=nil;
    n:=0;
    ntotal:=0;
    ArmarLista1 (L);
    inicializarVector(v);
    while (aux<>nil) do
    begin
        vcontador(v,aux);
        buscarporcentaje(aux,n,ntotal);
        Segundalista(L2,aux);
        aux:=aux^.sig;
    end;
    leercodigos(v);
end.