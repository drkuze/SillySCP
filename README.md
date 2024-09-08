# Silly SCP

This project is a modified version of the original code from [JayXTQ/SillySCP](https://github.com/JayXTQ/SillySCP/) and is licensed under the GPL-3.0 License.

## Changes:
- Configurable text for "No players online"
- Configurable color for the Discord embed (Hex color codes)
- Support for image and thumbnail through the configuration file
- Configurable status message template to display player count

## Configuration

You can set the following configuration options in your `config.yml` file:

- **Channel ID**: The ID of the channel where the bot will send messages. Example: `1279544677334253610`
- **Message ID**: The ID of the message that the bot will modify. Example: `1280910252325339311`
- **No Players Online Text**: Custom text to display when no players are online.
- **Embed Color**: Hex color code for the embed color (e.g., `#0000FF` for blue).
- **Image URL**: URL for an image to display in the embed.
- **Thumbnail URL**: URL for a thumbnail image to display in the embed.
- **Status Message Template**: A template for the status message. You can use `{0}` as a placeholder for the player count (e.g., `{0}/30 players active`).

## License
This project is licensed under the [GPL-3.0 License](https://www.gnu.org/licenses/gpl-3.0.txt).
