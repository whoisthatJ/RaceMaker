//
//  ISAppLovinRewardedVideoListener.h
//  ISAppLovinAdapter
//
//  Created by Daniil Bystrov on 3/15/16.
//  Copyright © 2016 IronSource. All rights reserved.
//

#import <Foundation/Foundation.h>

#import <ALSdk.h>
#import "ALIncentivizedInterstitialAd.h"

@protocol ISAppLovinRewardedVideoDelegate  <NSObject>
- (void)adUnitRVDidLoad:(ALAd *)ad;
- (void)adUnitRVDidFailToLoadWithError:(int)code;
- (void)adUnitRVFullyWatched:(ALAd *)ad;
- (void)adUnitRVRewardValidationRequestDidFailedWithError:(int)code ad:(ALAd *)ad;
- (void)adUnitRVStarted:(ALAd *)ad;
- (void)adUnitRVEnded:(ALAd *)ad;
- (void)adunitRVWasDisplayed:(ALAd *)ad;
- (void)adunitRVWasHidden:(ALAd *)ad;
- (void)adunitRVWasClicked:(ALAd *)ad;
@end

@interface ISAppLovinRewardedVideoListener : NSObject <ALAdLoadDelegate, ALAdDisplayDelegate, ALAdRewardDelegate, ALAdVideoPlaybackDelegate>
@property (nonatomic, weak) id<ISAppLovinRewardedVideoDelegate> delegate;
@end
