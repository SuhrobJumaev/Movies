# Movies
Final project for Alif Academy

Проект "Movies Catalog" представляет собой веб-приложение для удобного хранения и управления информацией о фильмах. Этот каталог обеспечивает простой и интуитивно понятный способ просмотра, добавления и управления фильмами, а также предоставляет расширенные функции для стриминга видео.

## Основные функции и возможности

- **Управление фильмами:**
  - Добавление новых фильмов.
  - Редактирование информации о фильмах.
  - Удаление фильмов.

- **Жанры:**
  - Выбор жанров для фильмов.
  - Создание, удаление и редактирование жанров.

- **Стриминг фильмов:**
  - Возможность стриминга фильмов.
  - Загрузка видео по частям в виде массива байт.

- **Управление пользователями:**
  - Создание, удаление и редактирование пользователей.
  - Выбор ролей для пользователей.

- **Роли:**
  - Создание, удаление и редактирование ролей.

- **Аутентификация и авторизация:**
  - Регистрация и вход в приложение.
  - Аутентификация по JWT токену.
  
- **API Health Check:**
  - Возможность проверки работоспособности API.

## Технологии
- **Backend:** ASP.NET Core 7. Трехуровневая архитектура.
- Movies.Web
- Movies.BusinessLogic
- Movies.DataAccess
- **Хранилище данных:** [PostgreSQL].

 ## Установка
 - Cклонируйте репозиторий: git clone https://github.com/SuhrobJumaev/Movies.git
 - Перейдите в директорию проекта: cd Movies
 - В дириектории Movies: выполнить команду docker-compose up --build
 - Подождать пока проект забилдится и запуститься в контейнере.
 - **Важно:** При первом запуске, после закачки всех образов докера, нужно подождать 1 или 2 минуты, пока запуститься контейнер с БД. После запуска БД, приложения автоматически создаст таблицы и создаст пользователя.
 - Приложения будет доступно по адресу: http://localhost:8080/swagger/index.html
 - Пользователь для авторизации: **Email:** test1@gmail.com **Password:** 123456
