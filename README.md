# RalfBot
[![CircleCI](https://circleci.com/gh/lawrab/RalfBot.svg?style=svg)](https://circleci.com/gh/lawrab/RalfBot)

RalfBot is a Discord bot that assists with the management of the Snail Racing community.

## Current features in progress
* Merging of Discord roles from multiple source roles, this feature assists with maintaining a single role which will include all users from multiple Streamer subscription roles

## Installation

Not ready to be installed yet, innitial commits are in progress and no deployed version available at this stage

## Usage

Bot commands will be added here when they are available

## Development

Publish
```sh
dotnet publish -c Release
docker build --pull -t lawrab/ralf-bot:alpine .
docker push lawrab/ralf-bot
```

Run local
```sh
docker run -e Discord__BotToken="YOUR TOKEN" -e DataPath="/Data" lawrab/ralf-bot
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)