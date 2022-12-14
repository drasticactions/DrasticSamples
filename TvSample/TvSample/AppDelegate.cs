using AVFoundation;
using System;
using MonoTouch.Dialog;
using CoreFoundation;
using CoreMedia;

namespace TvSample;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override UIWindow? Window
    {
        get;
        set;
    }

    readonly static NSString AVPlayerItem_Status = new NSString("status");
    readonly static NSString AVUrlAsset_Playable = new NSString("playable");
    readonly static NSString AVPlayer_CurrentItem = new NSString("currentItem");

    private RootElement menu;

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        this.Window = new UIWindow();

        this.menu = new RootElement("Test")
        {
            new Section ("Sample")
            {
                new StringElement ("再生", "1"),
                 new StringElement ("停止", "2"),
                  new StringElement ("スキップ送り", "3"),
                   new StringElement ("スキップ戻り", "4"),
            }
        };

        var dv = new NowPlayingViewController();

        // dv.OnSelection += Dv_OnSelection;

        this.Window!.RootViewController = new UIKit.UINavigationController(dv);
        this.Window!.MakeKeyAndVisible();
        player = new AVPlayer();
        addPeriodicTimeObserver();
        return true;
    }

    IntPtr observerContext = IntPtr.Zero;

    AVPlayerItem item;

    AVPlayer player;

    private async void Dv_OnSelection(NSIndexPath obj)
    {
        var test = menu[obj.Section][obj.Row];

        //player = AVPlayer.FromUrl(new NSUrl(""));
        if (test is StringElement ele)
        {
            if (ele.Value is "1")
            {
                this.item = AVPlayerItem.FromUrl(new NSUrl("https://cdn.discordapp.com/attachments/1021649119976103961/1052213688821620766/01_Codys_Theme.m4a"));
                item.AddObserver(this, AVPlayerItem_Status, NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.New, observerContext);
                player.ReplaceCurrentItemWithPlayerItem(item);
                player.Play();
            }
            else if (ele.Value == "2")
            {
                player.Pause();
            }
            else if (ele.Value == "3")
            {
                System.Diagnostics.Debug.WriteLine($"Time: {(long)player.CurrentTime.Seconds} Timescale: {player.CurrentTime.TimeScale}");
                var newValue = new CoreMedia.CMTime(((long)player.CurrentTime.Seconds + 10) * player.CurrentTime.TimeScale, player.CurrentTime.TimeScale);
                System.Diagnostics.Debug.WriteLine(newValue);
                this.item.Seek(newValue);
                // player.Seek(new CoreMedia.CMTime((long)player.CurrentTime.Seconds + 10, player.CurrentTime.TimeScale));
            }
            else if (ele.Value == "4")
            {
                var newValue = new CoreMedia.CMTime(((long)player.CurrentTime.Seconds - 10) * player.CurrentTime.TimeScale, player.CurrentTime.TimeScale);
                this.item.Seek(newValue);
            }
        }
    }

    public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
    {
        if (keyPath == AVPlayerItem_Status)
        {

        }
    }

    NSObject timeObservationToken;

    void addPeriodicTimeObserver()
    {
        if (timeObservationToken == null)
        {
            // Notify every half second
            var timeScale = new CMTimeScale(1000000000);

            var time = CMTime.FromSeconds(0.5, timeScale.Value);

            timeObservationToken = player?.AddPeriodicTimeObserver(time, DispatchQueue.MainQueue, handlePeriodicTimeObserver);
        }
    }

    void handlePeriodicTimeObserver(CMTime obj)
    {
        System.Diagnostics.Debug.WriteLine($"{obj}");

        //MPNowPlayingInfoCenter.DefaultCenter.NowPlaying.PlaybackProgress = obj.Value;
    }
}