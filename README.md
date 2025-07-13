# Final_Interview
# Final_Interview

A C# WPF application for visualizing BGR (Blue, Green, Red) image channels.  
Built with WPF and Emgu.CV (OpenCV for .NET).

## Features

- **Image Upload & Display:** Upload .jpg, .jpeg, .png, or .bmp files for conversion.
- **BGR Channel Visualization:** View blue, green, and red channels as separate images.
- **User Input Validation:** Requires user to enter a valid name (2–10 English letters).
- **Live Clock:** Continuously updates using a background thread.
- **Automatic Backup:** Backs up original images before processing.
- **Robust Image Verification:** Checks both file extension and "magic number" (file header).
- **Custom App Icon:** Application window features a custom icon.

## Key Classes & Functions

### `MainWindow.xaml.cs`
- Handles all UI events:
  - `NameBox_GotFocus`, `NameBox_LostFocus`, `NameBox_KeyDown` — manages name input and validation.
  - `btUPload_Click` — opens file dialog, triggers image processing.
  - `ProcessImage` — orchestrates backup, display, and BGR split.

### `Helpers.cs`
- **Image Processing:**
  - `MatToBitmap(Mat)` — converts Emgu.CV Mat to WPF BitmapImage.
  - `ShowBgrChannels(path, blueImg, greenImg, redImg)` — splits and displays B, G, R channels.
  - `IsImageFileMagic(path)` — validates file by magic number.
- **Validation:**
  - `IsValidImageExtension(string)` — checks supported extensions.
  - `IsValidName(string)` / `IsLettersOnly` / `IsNameLongEnough` — validates user input.
- **UI/Utility:**
  - `TurnVS(...)` — shows channel captions.
  - `Controltime(TextBlock)` — updates the clock using `DispatcherTimer` (WPF threading).
  - `updateClock(...)` — event handler for updating the clock.

### **Notable Libraries Used**
- **WPF (`System.Windows`):** For UI and threading (`DispatcherTimer` for clock updates).
- **Emgu.CV:** .NET wrapper for OpenCV — handles all image reading, writing, and channel splitting.

---

## How to Run

1. Open in **Visual Studio 2022** or newer (Windows).
2. Install NuGet package: `Emgu.CV`.
3. Click **Start** (F5) to run.

## Requirements

- **.NET 8.0 (Windows)**
- **Emgu.CV library**
- **Windows OS**

---

## Project Structure

- `MainWindow.xaml` / `MainWindow.xaml.cs` — WPF UI and logic
- `Helpers.cs` — Static utility class for processing/validation
- `README.md` — Project overview (this file)
- `appIcon (1).ico` — App icon

---

## Credits

Created by [liraz11111]

---

