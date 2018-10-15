# Movie Hunt

Just sample Xamarin app targeting The Movie Database API.

## Build and run

There no special instruction for building it. But you need to specify your API_KEY to access api.themoviedb.org. This can be done in App.xaml file.

## Used third-party libraries

- Prism (with DryIoc) — Used as MVVM framework to simplify common tasks, provide conventional-based linking of views with view-models and other cool features like these;
- Refit — To perform restful requests in type-safe and time-save manner;
- ModernHttpClient — Used in order to improve HttpClient's performance;
- System.Reactive — Used slightly (OperationCounter implemented on top of Observables), originally added Akavache for caching, which uses Observables as well, but then removed;
- Polly — Used for remote API requests retrying;