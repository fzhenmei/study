from System.Windows import Application
from System.Windows.Controls import UserControl
from System.Windows.Media.Animation import ClockState
class App:
	def __init__(self):
		self.root = Application.Current.LoadRootVisual(UserControl(), "app.xaml")
		self.root.animationButton.Click += self.startStopAnimation
		self.root.pauseButton.Click += self.pauseAnimation
		self.isPaused = False
	def startStopAnimation(self,s,e):
		if self.root.rectAnimation.GetCurrentState() == ClockState.Stopped:
			self.root.rectAnimation.Begin()
			self.root.animationButton.Content = "Stop Animation"
		else:
			self.root.rectAnimation.Stop()
			self.root.animationButton.Content = "Start Animation"
			self.root.pauseButton.Content = "Pause Animation"
	def pauseAnimation(self,s,e):		
		if self.root.rectAnimation.GetCurrentState() == ClockState.Active and not self.isPaused is True:
			self.root.rectAnimation.Pause()
			self.isPaused = True
			self.root.pauseButton.Content = "Resume Animation"
		else:
			self.root.rectAnimation.Resume()
			self.isPaused = False
			self.root.pauseButton.Content = "Pause Animation"
App()