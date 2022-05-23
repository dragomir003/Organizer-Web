create database Organizer;

use Organizer;

create table Korisnik(
	id int identity(1, 1) primary key,
	username varchar(32) not null unique
);

insert into Korisnik(username) values
('dragomir'),
('u1'),
('u2'),
('u3');

create table Projekat(
	id int identity(1, 1) primary key,
	naziv varchar(32) not null unique,
	opis varchar(512),
	pocetak datetime not null,
	kraj datetime,
	ocekivaniKraj datetime
);

insert into Projekat(naziv, opis, pocetak) values
('organizer', 'Aplikacija koja prati razvitak razlicitih projekata', '2022-04-08'),
('p1', 'p1 opis', CURRENT_TIMESTAMP),
('p2', 'p2 opis', CURRENT_TIMESTAMP),
('p3', 'p3 opis', CURRENT_TIMESTAMP);


create table Clanstvo (
	id int identity(1, 1) primary key,
	projekat int not null,
	korisnik int not null,
	ovlascenja int not null check (ovlascenja >= 0 and ovlascenja < 3) -- Ovlascenje 2 je vlasnik, 1 je bitan ucesnik, a 0 je obican smrtnik.
	foreign key (projekat) references Projekat(id),
	foreign key (korisnik) references Korisnik(id),
);

insert into Clanstvo(projekat, korisnik, ovlascenja) values
(1, 1, 2),
(2, 2, 2),
(3, 3, 2),
(4, 4, 2),
(1, 3, 0);

go

create function ValidateLogin (@username varchar(32))
returns int as
begin
declare @cnt int;

set @cnt = (select count(*) from Korisnik where username = @username);

return abs(@cnt - 1);

end;

go

create proc Register
@username varchar(32) as
begin try
	insert into Korisnik(username) values (@username);
	return 0;
end try
begin catch
	return 1;
end catch;

go

create function GetProjectsBasic (@username varchar(32)) returns table as
return (select Projekat.id as id, Projekat.naziv as naziv, Projekat.opis as opis from Clanstvo
join Projekat on Projekat.id = Clanstvo.projekat
join Korisnik on Korisnik.id = Clanstvo.korisnik
where Korisnik.username = @username);

go

create proc DodajClanstvoBasic
@korisnik int,
@projekat int
as
begin
if not exists (select korisnik, projekat from Clanstvo where korisnik = @korisnik and projekat = @projekat)
	insert into Clanstvo(korisnik, projekat, ovlascenja) values (@korisnik, @projekat, 1)
end;

go

create function ClanoviProjekta(@projekat int) returns table as
return select Korisnik.id as id, Korisnik.username as username from Clanstvo join Korisnik on Clanstvo.korisnik = korisnik.id where clanstvo.projekat = @projekat;
